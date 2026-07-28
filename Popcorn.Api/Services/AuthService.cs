using BC = BCrypt.Net.BCrypt;
using Popcorn.Api.Data;
using Popcorn.Api.Models.Entities;
using Popcorn.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace Popcorn.Api.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _dbContext;
		private readonly IHttpClientFactory _factory;

		public AuthService(AppDbContext dbContext, IHttpClientFactory factory)
		{
			_dbContext = dbContext;
			_factory = factory;
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
	}
}
