using System.Text.Json.Serialization;

namespace Popcorn.Api.Models
{
	public class TmdbResponse<T>
	{
		[JsonPropertyName("page")]
		public int Page { get; set; }
		[JsonPropertyName("results")]
		public List<T> Results { get; set; } = new();
		[JsonPropertyName("total_pages")]
		public int TotalPages { get; set; }
		[JsonPropertyName("total_results")]
		public int TotalResults { get; set; }
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "media_type")]
	[JsonDerivedType(typeof(TmdbMovie), typeDiscriminator: "movie")]
	[JsonDerivedType(typeof(TmdbSerie), typeDiscriminator: "tv")]
	[JsonDerivedType(typeof(TmdbPerson), typeDiscriminator: "person")]
	public abstract class TmdbResult
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
	}

	public class TmdbMovie : TmdbResult
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;
		[JsonPropertyName("overview")]
		public string Overview { get; set; } = string.Empty;
		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }
	}

	public class TmdbSerie : TmdbResult
	{
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("overview")]
		public string Overview { get; set; } = string.Empty;
		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }
	}

	public class TmdbPerson : TmdbResult
	{
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("profile_path")]
		public string? ProfilePath { get; set; }
	}
}
