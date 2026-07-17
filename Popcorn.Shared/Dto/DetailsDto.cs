namespace Popcorn.Shared.Dto
{
	public class MovieDetailsDto
	{
		public int Id { get; set; }
		public string HomePage { get; set; } = string.Empty;
		public string Overview { get; set; } = string.Empty;
		public string PosterPath { get; set; } = string.Empty;
		public string ReleaseDate { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
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
		public string HomePage { get; set; } = string.Empty;
	}

	public class PersonDetailsDto
	{
		public int Id { get; set; }
		public string Biography { get; set; } = string.Empty;
		public string Birthday { get; set; } = string.Empty;
		public string Deathday { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Homepage { get; set; } = string.Empty;
		public string PlaceOfBirth { get; set; } = string.Empty;
		public string ProfilePath { get; set; } = string.Empty;
		public string KnownForDepartment { get; set; } = string.Empty;
		public int? Age { get; set; }
	}
}
