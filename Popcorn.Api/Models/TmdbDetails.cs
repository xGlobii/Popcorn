using System.Text.Json.Serialization;

namespace Popcorn.Api.Models
{
	public class TmdbMovieDetails
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("homepage")]
		public string? HomePage { get; set; }
		[JsonPropertyName("overview")]
		public string Overview { get; set; } = string.Empty;
		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }
		[JsonPropertyName("release_date")]
		public string ReleaseDate { get; set; } = string.Empty;
		[JsonPropertyName("status")]
		public string Status { get; set; } = string.Empty;
		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;
	}

	public class TmdbTvSerieDetails
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("first_air_date")]
		public string FirstAirDate { get; set; } = string.Empty;
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("number_of_episodes")]
		public int NumberOfEpisodes { get; set; }
		[JsonPropertyName("number_of_seasons")]
		public int NumberOfSeasons { get; set; }
		[JsonPropertyName("overview")]
		public string Overview { get; set; } = string.Empty;
		[JsonPropertyName("status")]
		public string Status { get; set; } = string.Empty;
		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }
		[JsonPropertyName("homepage")]
		public string? HomePage { get; set; }
		[JsonPropertyName("seasons")]
		public List<Season> Seasons { get; set; } = new();
	}

	public class Season
	{
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("overview")]
		public string? Overview { get; set; }
		[JsonPropertyName("season_number")]
		public int SeasonNumber { get; set; }
	}

	public class EpisodesDetails
	{
		[JsonPropertyName("episodes")]
		public List<Episode> Episodes { get; set; } = new();
	}

	public class Episode
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("overview")]
		public string? Overview { get; set; }
		[JsonPropertyName("still_path")]
		public string ImagePath { get; set; } = string.Empty;
		[JsonPropertyName("episode_number")]
		public int EpisodeNumber { get; set; }
	}

	public class TmdbPersonDetails
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("biography")]
		public string Biography { get; set; } = string.Empty;
		[JsonPropertyName("birthday")]
		public string? Birthday { get; set; }
		[JsonPropertyName("deathday")]
		public string? Deathday { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("homepage")]
		public string? Homepage { get; set; }
		[JsonPropertyName("place_of_birth")]
		public string PlaceOfBirth { get; set; } = string.Empty;
		[JsonPropertyName("profile_path")]
		public string? ProfilePath { get; set; }
		[JsonPropertyName("known_for_department")]
		public string KnownForDepartment { get; set; } = string.Empty;
	}
}
