using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IAuthService
	{
		public Task<bool> Register(RegisterDto dto);
		public Task<AuthTokensDto?> Login(LoginDto dto);
		public Task<AuthTokensDto?> Refresh(AuthTokensDto dto);
	}
}
