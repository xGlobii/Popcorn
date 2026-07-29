using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IAuthService
	{
		public Task<bool> Register(RegisterDto dto);
		public Task<AuthResponseDto?> Login(LoginDto dto);
	}
}
