namespace Popcorn.Api.Models.Entities
{
	public enum Status
	{
		ToWatch,
		Watched
	}

	public class ActivityItem
	{
		public int TmdbId { get; set; }
		public string MediaType { get; set; } = string.Empty;
		public TmdbMedia? MediaItem { get; set; }
		public Guid UserId { get; set; }
		public User? User { get; set; }
		public Status Status { get; set; }
		public DateOnly AddedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
	}
}
