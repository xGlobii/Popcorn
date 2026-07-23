using System.ComponentModel;
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
		[JsonPropertyName("credits")]
		public Credits Credits { get; set; } = new();
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
		[JsonPropertyName("aggregate_credits")]
		public AggregateCredits Credits { get; set; } = new();
	}

	public class Credits
	{
		[JsonPropertyName("cast")]
		public List<SimplePerson> Cast { get; set; } = new();
	}

	public class AggregateCredits
	{
		[JsonPropertyName("cast")]
		public List<TvSeriePerson> Cast { get; set; } = new();
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
		[JsonPropertyName("guest_stars")]
		public List<SimplePerson> GuestStars { get; set; } = new();
	}

	public class TvSeriePerson
	{
		[JsonPropertyName("known_for_department")]
		public string Department { get; set; } = string.Empty;
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("profile_path")]
		public string? ProfilePath { get; set; }
		[JsonPropertyName("roles")]
		public List<Role> Roles { get; set; } = new();
	}

	public class SimplePerson
	{
		[JsonPropertyName("known_for_department")]
		public string Department { get; set; } = string.Empty;
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("profile_path")]
		public string? ProfilePath { get; set; }
		[JsonPropertyName("character")]
		public string Character { get; set; } = string.Empty;
	}

	public class Role
	{
		[JsonPropertyName("character")]
		public string Character { get; set; } = string.Empty;
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

	public class CombinedCredits
	{
		[JsonPropertyName("crew")]
		public List<CreditsItem> Crew { get; set; } = new();
		[JsonPropertyName("cast")]
		public List<CreditsItem> Cast { get; set; } = new();
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "media_type")]
	[JsonDerivedType(typeof(MovieCredits), typeDiscriminator: "movie")]
	[JsonDerivedType(typeof(TvSerieCredits), typeDiscriminator: "tv")]
	public abstract class CreditsItem
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("overview")]
		public string? Overview { get; set; }
		[JsonPropertyName("vote_count")]
		public int VoteCount { get; set; }
		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }
		[JsonPropertyName("job")]
		public string? Job { get; set; }
	}

	public class MovieCredits : CreditsItem
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;
		[JsonPropertyName("release_date")]
		public string ReleaseDate { get; set; } = string.Empty;
	}

	public class TvSerieCredits : CreditsItem
	{
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("first_air_date")]
		public string AirDate { get; set; } = string.Empty;
	}
}
