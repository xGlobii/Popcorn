using Popcorn.Shared.Enums;

namespace Popcorn.Shared.Dto
{
	public class LibraryDto
	{
		public int TmdbId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
		public DateOnly AddedAt { get; set; }
		public ActivityStatus Status { get; set; }
	}
}
