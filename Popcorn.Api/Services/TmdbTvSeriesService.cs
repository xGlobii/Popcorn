using Popcorn.Api.Dto;
using Popcorn.Api.Models;
using System.Net;

namespace Popcorn.Api.Services
{
	public class TmdbTvSeriesService : ITvSeriesService
	{
		private readonly IHttpClientFactory _factory;

		public TmdbTvSeriesService(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<TvSerieDetailsDto?> GetTvSerieDetails(int id)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"tv/{id}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!response.IsSuccessStatusCode)
			{
				response.EnsureSuccessStatusCode();
			}

			var result = await response.Content.ReadFromJsonAsync<TmdbTvSerieDetails>();

			if (result == null)
				return null;

			return new TvSerieDetailsDto
			{
				Id = result.Id,
				Name = result.Name,
				FirstAirDate = result.FirstAirDate,
				NumberOfEpisodes = result.NumberOfEpisodes,
				NumberOfSeasons = result.NumberOfSeasons,
				Overview = result.Overview,
				Status = result.Status,
				PosterPath = result.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{result.PosterPath}"
			};
		}
	}
}
