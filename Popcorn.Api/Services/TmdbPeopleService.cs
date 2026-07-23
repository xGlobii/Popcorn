using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace Popcorn.Api.Services
{
	public class TmdbPeopleService : IPeopleService
	{
		private readonly IHttpClientFactory _factory;
		private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions();

		public TmdbPeopleService(IHttpClientFactory factory)
		{
			_factory = factory;
			_jsonOptions.AllowOutOfOrderMetadataProperties = true;
		}

		public async Task<PersonDetailsDto?> GetPersonDetails(int id)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"person/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!response.IsSuccessStatusCode)
			{
				response.EnsureSuccessStatusCode();
			}

			var result = await response.Content.ReadFromJsonAsync<TmdbPersonDetails>();

			if (result == null)
			{
				return null;
			}

			return new PersonDetailsDto
			{
				Id = result.Id,
				Biography = result.Biography,
				Birthday = result.Birthday == null ? "" : result.Birthday,
				Deathday = result.Deathday == null ? "present" : result.Deathday,
				Homepage = result.Homepage == null ? null : result.Homepage,
				KnownForDepartment = result.KnownForDepartment,
				Name = result.Name,
				PlaceOfBirth = result.PlaceOfBirth,
				ProfilePath = result.ProfilePath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{result.ProfilePath}",
				Age = CalculateAge(result.Deathday == null ? null : DateTime.Parse(result.Deathday), result.Birthday == null ? null : DateTime.Parse(result.Birthday))
			};
		}

		private int? CalculateAge(DateTime? deathday, DateTime? birthday)
		{
			int age = 0;

			deathday ??= DateTime.UtcNow;

			if (birthday == null)
				return null;

			age = deathday.Value.Year - birthday.Value.Year;

			if (deathday.Value.Month < birthday.Value.Month)
				age -= 1;
			else if (deathday.Value.Month == birthday.Value.Month && deathday.Value.Day < birthday.Value.Day)
				age -= 1;

			return age;
		}

		public async Task<IEnumerable<CreditsDto>?> GetFilmographyPreview(int id)
		{
			CombinedCredits? credits = await GetCredits(id);

			if (credits == null)
				return null;

			List<CreditsDto> dto = MapToDto(credits.Cast.OrderByDescending(c => c.VoteCount).Take(10));

			return dto;
		}

		private async Task<CombinedCredits?> GetCredits(int id)
		{
			var client = _factory.CreateClient("TMDB");
			var respond = await client.GetAsync($"person/{id}/combined_credits");

			if (respond.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!respond.IsSuccessStatusCode)
			{
				respond.EnsureSuccessStatusCode();
			}

			var result = await respond.Content.ReadFromJsonAsync<CombinedCredits>(_jsonOptions);

			return result;
		}

		public async Task<CombinedCreditsDto?> GetFilmography(int id)
		{
			CombinedCredits? credits = await GetCredits(id);

			if (credits == null)
				return null;

			List<CreditsDto> cast = MapToDto(credits.Cast);
			List<CreditsDto> crew = MapToDto(credits.Crew);

			return new CombinedCreditsDto
			{
				Cast = cast,
				Crew = crew
			};
		}

		private List<CreditsDto> MapToDto(IEnumerable<CreditsItem> respond)
		{
			List<CreditsDto>? dto = new();

			foreach (var media in respond)
			{
				switch (media)
				{
					case MovieCredits movie:
						dto.Add(new CreditsDto
						{
							Id = movie.Id,
							Overview = movie.Overview == null ? "" : movie.Overview,
							PosterPath = movie.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}",
							ReleaseDate = movie.ReleaseDate,
							Title = movie.Title,
							MediaType = "movie",
							Job = movie.Job == null ? "Actor" : movie.Job
						});
						break;
					case TvSerieCredits tvSerie:
						dto.Add(new CreditsDto
						{
							Id = tvSerie.Id,
							Overview = tvSerie.Overview == null ? "" : tvSerie.Overview,
							PosterPath = tvSerie.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{tvSerie.PosterPath}",
							ReleaseDate = tvSerie.AirDate,
							Title = tvSerie.Name,
							MediaType = "tv",
							Job = tvSerie.Job == null ? "Actor" : tvSerie.Job
						});
						break;
				}
			}

			return dto;
		}
	}
}
