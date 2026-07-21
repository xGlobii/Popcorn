using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
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

			List<SeasonDto> seasons = new();

			foreach (var season in result.Seasons)
			{
				if (season.SeasonNumber != 0)
				{
					seasons.Add(new SeasonDto
					{
						Name = season.Name,
						Overview = season.Overview == null ? "" : season.Overview,
						SeasonNumber = season.SeasonNumber
					});
				}
			}

			return new TvSerieDetailsDto
			{
				Id = result.Id,
				Name = result.Name,
				FirstAirDate = result.FirstAirDate,
				NumberOfEpisodes = result.NumberOfEpisodes,
				NumberOfSeasons = result.NumberOfSeasons,
				Overview = result.Overview,
				Status = result.Status,
				PosterPath = result.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{result.PosterPath}",
				HomePage = result.HomePage == null ? "" : result.HomePage,
				Seasons = seasons
			};
		}

		public async Task<EpisodesDetailsDto?> GetEpisodesDetails(int id, int seasonNumber)
		{
			var client = _factory.CreateClient("TMDB");
			var resposne = await client.GetAsync($"tv/{id}/season/{seasonNumber}");

			if(resposne.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if(!resposne.IsSuccessStatusCode)
			{
				resposne.EnsureSuccessStatusCode();
			}

			var result = await resposne.Content.ReadFromJsonAsync<EpisodesDetails>();

			if (result == null)
				return null;

			List<EpisodeDto> episodes = new();

			foreach(var episode in result.Episodes)
			{
				episodes.Add(new EpisodeDto
				{
					Id = episode.Id,
					ImagePath = $"https://image.tmdb.org/t/p/w500{episode.ImagePath}",
					Name = episode.Name,
					Overview = episode.Overview == null ? "" : episode.Overview,
					EpisodeNumber = episode.EpisodeNumber
				});
			}

			return new EpisodesDetailsDto
			{
				Episodes = episodes
			};
		}
	}
}
