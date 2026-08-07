namespace Popcorn.Api.Models.Entities
{
	public class TmdbMedia
	{
		public int TmdbId { get; set; }
		public string MediaType { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string PosterPath { get; set; } = string.Empty;
		public ICollection<ActivityItem> UserMediaItems { get; set; } = new List<ActivityItem>();
	}
}
