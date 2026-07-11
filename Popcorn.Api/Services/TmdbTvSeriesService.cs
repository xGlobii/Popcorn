using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;

namespace Popcorn.Api.Services
{
	public class TmdbTvSeriesService : ITvSeriesService
	{
		private readonly IHttpClientFactory _factory;

		public TmdbTvSeriesService(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<TmdbSerieDetails?> GetTvSerieDetails(int id)
		{
			var client = _factory.CreateClient("TMDB");
			var respond = await client.GetAsync($"tv/{id}");

			respond.EnsureSuccessStatusCode();

			var result = await respond.Content.ReadFromJsonAsync<TmdbSerieDetails>();

			if (result != null)
				return result;
			else
				return null;
		}
	}
}
