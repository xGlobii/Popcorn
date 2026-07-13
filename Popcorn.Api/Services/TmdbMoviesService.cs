using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using System.Net;

namespace Popcorn.Api.Services
{
	public class TmdbMoviesService : IMoviesService
	{
		private readonly IHttpClientFactory _factory;

		public TmdbMoviesService(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<MovieDetailsDto?> GetMovieDetails(int id)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"movie/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!response.IsSuccessStatusCode)
			{
				response.EnsureSuccessStatusCode();
			}

			var result = await response.Content.ReadFromJsonAsync<TmdbMovieDetails>();

			if (result == null)
				return null;

			return new MovieDetailsDto
			{
				Id = result.Id,
				HomePage = result.HomePage == null ? "" : result.HomePage,
				Overview = result.Overview,
				PosterPath = result.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{result.PosterPath}",
				ReleaseDate = result.ReleaseDate,
				Status = result.Status,
				Title = result.Title
			};
		}
	}
}
