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
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

			if (user == null)
				return null;

			if (BC.Verify(dto.Password, user.HashedPassword))
			{
				var token = GenerateJwtToken(user.Id.ToString());

				if (string.IsNullOrEmpty(token))
					return null;

				byte[] randomBytes = new byte[32];
				RandomNumberGenerator.Fill(randomBytes);
				string refreshToken = Convert.ToBase64String(randomBytes);
				string hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

				try
				{
					await _dbContext.UserSessions.AddAsync(new UserSession
					{
						RefreshToken = hashedToken,
						ExpireAt = DateTime.UtcNow.AddDays(7),
						User = user
					});

					await _dbContext.SaveChangesAsync();

					return new AuthTokensDto
					{
						RefreshToken = refreshToken,
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

		private string GenerateJwtToken(string userId)
		{
			var key = _config["Auth:SecurityKey"];
			if (key == null)
				return string.Empty;

			var header = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));

			List<Claim> claims = new();

			claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));

			var payload = new JwtPayload(
				issuer: null,
				audience: null,
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddMinutes(15));

			var securityToken = new JwtSecurityToken(header, payload);

			var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

			return token;
		}
	}
}
