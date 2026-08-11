using BC = BCrypt.Net.BCrypt;
using Popcorn.Api.Data;
using Popcorn.Api.Models.Entities;
using Popcorn.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Reflection.Metadata;

namespace Popcorn.Api.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _dbContext;
		private readonly IConfiguration _config;

		public AuthService(AppDbContext dbContext, IConfiguration config)
		{
			_dbContext = dbContext;
			_config = config;
		}

		public async Task<bool> Register(RegisterDto dto)
		{
			if (await _dbContext.Users.AnyAsync(u => u.Username == dto.Username))
				return false;

			if (await _dbContext.Users.AnyAsync(u => u.Email == dto.Email))
				return false;

			string passwordHash = BC.HashPassword(dto.Password);

			try
			{
				await _dbContext.AddAsync(new User
				{
					Email = dto.Email,
					HashedPassword = passwordHash,
					Username = dto.Username
				});

				await _dbContext.SaveChangesAsync();
				return true;

			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<AuthTokensDto?> Login(LoginDto dto)
		{
			var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == dto.Email);

			if (user == null)
				return null;

			if (BC.Verify(dto.Password, user.HashedPassword))
			{
				var sessionId = Guid.NewGuid();

				var token = GenerateJwtToken(user.Id.ToString(), user.Username, sessionId);

				if (string.IsNullOrEmpty(token))
					return null;

				var refreshToken = GenerateRefreshToken();

				try
				{
					await _dbContext.UserSessions.AddAsync(new UserSession
					{
						RefreshToken = refreshToken.hashedToken,
						ExpireAt = DateTime.UtcNow.AddDays(7),
						UserId = user.Id,
						SessionId = sessionId
					});

					await _dbContext.SaveChangesAsync();

					return new AuthTokensDto
					{
						RefreshToken = refreshToken.refreshToken,
						Token = token
					};
				}
				catch (Exception)
				{
					return null;
				}
			}

			return null;
		}

		private string GenerateJwtToken(string userId, string username, Guid sessionId)
		{
			var key = _config["Auth:SecurityKey"];
			if (key == null)
				return string.Empty;

			var header = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));

			List<Claim> claims = new();

			claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));
			claims.Add(new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, username));

			var payload = new JwtPayload(
				issuer: "Popcorn",
				audience: "Popcorn.Api",
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddMinutes(15));

			var securityToken = new JwtSecurityToken(header, payload);

			var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

			return token;
		}

		private (string refreshToken, string hashedToken) GenerateRefreshToken()
		{
			byte[] randomBytes = new byte[32];
			RandomNumberGenerator.Fill(randomBytes);
			string refreshToken = Convert.ToBase64String(randomBytes);
			string hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

			return (refreshToken, hashedToken);
		}

		public async Task<AuthTokensDto?> Refresh(AuthTokensDto dto)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			var key = _config["Auth:SecurityKey"];
			if (key == null)
				return null;

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

			var validation = await tokenHandler.ValidateTokenAsync(dto.Token, new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = "Popcorn",
				ValidateAudience = true,
				ValidAudience = "Popcorn.Api",
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = securityKey
			});

			if(!validation.IsValid)
			{
				return null;
			}

			var token = tokenHandler.ReadJwtToken(dto.Token);

			if (!Guid.TryParse(token.Payload.Sub, out Guid userId))
			{
				return null;
			}

			string refreshToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.RefreshToken)));

			var usernameClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

			if (usernameClaim == null)
				return null;

			var sessionIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);

			if (sessionIdClaim == null)
				return null;

			if (!Guid.TryParse(sessionIdClaim.Value, out Guid sessionId))
			{
				return null;
			}

			var result = await _dbContext.UserSessions.FirstOrDefaultAsync(us => us.UserId == userId && us.RefreshToken == refreshToken);

			if (result == null)
				return null;

			if (result.IsRevoked)
			{
				var userSessions = await _dbContext.UserSessions.Where(us => us.UserId == userId && us.SessionId == sessionId).ExecuteUpdateAsync(setter => setter.SetProperty(p => p.IsRevoked, true));
				return null;
			}

			if(result.ExpireAt <= DateTime.UtcNow)
			{
				return null;
			}

			var newToken = GenerateJwtToken(token.Payload.Sub, usernameClaim.Value, sessionId);
			var newRefreshToken = GenerateRefreshToken();

			try
			{
				result.IsRevoked = true;

				await _dbContext.UserSessions.AddAsync(new UserSession
				{
					RefreshToken = newRefreshToken.hashedToken,
					ExpireAt = DateTime.UtcNow.AddDays(7),
					UserId = userId,
					SessionId = sessionId
				});

				await _dbContext.SaveChangesAsync();

				return new AuthTokensDto
				{
					RefreshToken = newRefreshToken.refreshToken,
					Token = newToken
				};
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<bool> Logout(Guid userId, Guid sessionId)
		{
			try
			{
				await _dbContext.UserSessions.Where(u => u.UserId == userId && u.IsRevoked == false && u.SessionId == sessionId).ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsRevoked, true));

				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
	}
}
