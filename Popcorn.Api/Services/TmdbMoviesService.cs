using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;

namespace Popcorn.Api.Services
{
	public class TmdbMoviesService : IMoviesService
	{
		private readonly IHttpClientFactory _factory;

		public TmdbMoviesService(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<TmdbMovieDetails?> GetMovieDetails(int id)
		{
			var client = _factory.CreateClient("TMDB");
			var respond = await client.GetAsync($"movie/{id}");

			respond.EnsureSuccessStatusCode();

			var result = await respond.Content.ReadFromJsonAsync<TmdbMovieDetails>();
			if (result != null)
				return result;
			else
				return null;

		}
	}
}
