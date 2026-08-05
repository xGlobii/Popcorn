using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface ILibraryService
	{
		public Task<List<LibraryDto>?> GetLibraryItemsAsync(Guid userId);
	}
}
