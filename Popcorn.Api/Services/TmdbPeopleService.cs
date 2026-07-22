using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using System.Net;

namespace Popcorn.Api.Services
{
	public class TmdbPeopleService : IPeopleService
	{
		private readonly IHttpClientFactory _factory;

		public TmdbPeopleService(IHttpClientFactory factory)
		{
			_factory = factory;
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
	}
}
