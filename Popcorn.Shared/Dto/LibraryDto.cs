namespace Popcorn.Shared.Dto
{
	public class LibraryDto
	{
		public required IReadOnlyList<MediaItemDto> Items { get; set; }
		public int TotalPages { get; set; }
	}
}
