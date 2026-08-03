using Popcorn.Shared.Enums;
using System.Data;
using System.Text.Json.Serialization;

namespace Popcorn.Shared.Dto
{
	public class MovieDetailsDto
	{
		public int Id { get; set; }
		public string? HomePage { get; set; }
		public string Overview { get; set; } = string.Empty;
		public string PosterPath { get; set; } = string.Empty;
		public string ReleaseDate { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public ActivityStatus MediaStatus { get; set; } = ActivityStatus.None;
		public required IReadOnlyList<PersonDto> Cast { get; set; }
	}

	public class TvSerieDetailsDto
	{
		public int Id { get; set; }
		public string FirstAirDate { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public int NumberOfEpisodes { get; set; }
		public int NumberOfSeasons { get; set; }
		public string Overview { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public string PosterPath { get; set; } = string.Empty;
		public string? HomePage { get; set; }
		public ActivityStatus MediaStatus { get; set; } = ActivityStatus.None;
		public required IReadOnlyList<SeasonDto> Seasons { get; set; }
		public required IReadOnlyList<PersonDto> Cast { get; set; }
	}

	public class SeasonDto
	{
		public string Name { get; set; } = string.Empty;
		public string Overview { get; set; } = string.Empty;
		public int SeasonNumber { get; set; }
	}

	public class EpisodesDetailsDto
	{
		public required IReadOnlyList<EpisodeDto> Episodes { get; set; }
	}

	public class EpisodeDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Overview { get; set; } = string.Empty;
		public string ImagePath { get; set; } = string.Empty;
		public int EpisodeNumber { get; set; }
		public required IReadOnlyList<PersonDto> GuestStars { get; set; }
	}

	public class PersonDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string ProfilePath { get; set; } = string.Empty;
		public string Character { get; set; } = string.Empty;
	}

	public class PersonDetailsDto
	{
		public int Id { get; set; }
		public string Biography { get; set; } = string.Empty;
		public string Birthday { get; set; } = string.Empty;
		public string Deathday { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string? Homepage { get; set; }
		public string PlaceOfBirth { get; set; } = string.Empty;
		public string ProfilePath { get; set; } = string.Empty;
		public string KnownForDepartment { get; set; } = string.Empty;
		public int? Age { get; set; }
	}

	public class CombinedCreditsDto
	{
		public required IReadOnlyList<CreditsDto> Crew { get; set; }
		public required IReadOnlyList<CreditsDto> Cast { get; set; }
	}

	public class CreditsDto
	{
		public int Id { get; set; }
		public string Overview { get; set; } = string.Empty;
		public string PosterPath { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string ReleaseDate { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
		public string Job { get; set; } = string.Empty;
	}
}
