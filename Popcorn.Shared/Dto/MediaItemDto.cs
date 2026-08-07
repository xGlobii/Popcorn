namespace Popcorn.Shared.Dto
{
	public class MediaItemDto
	{
		public int TmdbId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
		public DateOnly AddedAt { get; set; }
		public string Status { get; set; } = string.Empty;
		public string PosterPath { get; set; } = string.Empty;
	}
}
