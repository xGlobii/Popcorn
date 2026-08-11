namespace Popcorn.Api.Models.Entities
{
	public class UserSession
	{
		public int Id { get; set; }
		public string RefreshToken { get; set; } = string.Empty;
		public DateTime ExpireAt { get; set; }
		public bool IsRevoked { get; set; } = false;
		public Guid UserId { get; set; }
		public User? User { get; set; }
		public Guid SessionId { get; set; }
	}
}
