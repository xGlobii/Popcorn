using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IProfileService
	{
		public Task<ProfileDto?> GetDetailsAsync(Guid userId);
	}
}
