using Popcorn.Shared.Enums;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IActivityService
	{
		public Task<bool> UpsertAsync(ActivityDto dto, Guid userId);
		public Task<bool> DeleteAsync(string mediaType, int id, Guid userId);
	}
}
