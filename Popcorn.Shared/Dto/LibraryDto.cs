using Popcorn.Shared.Enums;

namespace Popcorn.Shared.Dto
{
	public class LibraryDto
	{
		public required IReadOnlyList<LibraryItemDto> Items { get; set; }
		public int TotalPages { get; set; }
	}

	public class LibraryItemDto
	{
		public int TmdbId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
		public DateOnly AddedAt { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}
