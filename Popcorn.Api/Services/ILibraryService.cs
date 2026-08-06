using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface ILibraryService
	{
		public Task<LibraryDto?> GetLibraryItemsAsync(Guid userId, int page, string query);
	}
}
