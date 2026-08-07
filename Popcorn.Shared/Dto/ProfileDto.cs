namespace Popcorn.Shared.Dto
{
	public class ProfileDto
	{
		public string Username { get; set; } = string.Empty;
		public int ToWatchCount { get; set; }
		public int WatchedCount { get; set; }
		public int MoviesCount { get; set; }
		public int TvSeriesCount { get; set; }
		public required IReadOnlyList<MediaItemDto> Movies { get; set; }
		public required IReadOnlyList<MediaItemDto> TvSeries { get; set; }
	}
}
