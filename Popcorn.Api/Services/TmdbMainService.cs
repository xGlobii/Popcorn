using Microsoft.AspNetCore.Mvc.RazorPages;
using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using System.Net;

namespace Popcorn.Api.Services
{
	public class TmdbMainService : IMainService
	{
		private readonly Random random = new Random();
		private readonly IHttpClientFactory _factory;

		public TmdbMainService(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<MainPageDto?> GetMainPage()
		{
			List<string> types = new() { "movie", "tv" };
			int typeIndex = random.Next(types.Count);
			int page = random.Next(1, 501);

			Task<ResponseItemDto?> discover;
			Task<List<ResponseItemDto>?> nowPlaying;

			switch (types[typeIndex])
			{
				case "movie":
					discover = GetDiscoveryData<TmdbMovie>($"discover/{types[typeIndex]}?page={page}");
					break;
				case "tv":
					discover = GetDiscoveryData<TmdbSerie>($"discover/{types[typeIndex]}?page={page}");
					break;
				default:
					throw new InvalidOperationException("Unknown type");
			}

			nowPlaying = GetNowPlaying();

			await Task.WhenAll(discover, nowPlaying);

			var discoverData = await discover;
			var nowPlayingData = await nowPlaying;

			if (discoverData == null || nowPlayingData == null)
				return null;

			return new MainPageDto
			{
				Discover = discoverData,
				NowPlaying = nowPlayingData
			};
		}

		private async Task<ResponseItemDto?> GetDiscoveryData<T>(string url) where T : TmdbResult
		{
			var client = _factory.CreateClient("TMDB");
			var respond = await client.GetAsync(url);

			if (respond.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!respond.IsSuccessStatusCode)
			{
				respond.EnsureSuccessStatusCode();
			}

			var result = await respond.Content.ReadFromJsonAsync<TmdbResponse<T>>();

			if (result == null)
				return null;

			int item = random.Next(Math.Min(20, result.Results.Count));

			return MapToDto(result.Results[item]);
		}

		private async Task<List<ResponseItemDto>?> GetNowPlaying()
		{
			var client = _factory.CreateClient("TMDB");
			var respond = await client.GetAsync("movie/now_playing");

			if (respond.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!respond.IsSuccessStatusCode)
			{
				respond.EnsureSuccessStatusCode();
			}

			var result = await respond.Content.ReadFromJsonAsync<TmdbResponse<TmdbMovie>>();

			if (result == null)
				return null;

			return result.Results.Take(4).Select(MapToDto).OfType<ResponseItemDto>().ToList();
		}

		private ResponseItemDto? MapToDto(TmdbResult? result)
		{
			if (result == null)
				return null;

			ResponseItemDto dto = new();

			switch (result)
			{
				case TmdbMovie movie:
					dto = new ResponseItemDto
					{
						Id = movie.Id,
						ImagePath = movie.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}",
						MediaType = "movie",
						Name = movie.Title,
						Overview = movie.Overview,
						Url = $"movies/{movie.Id}"
					};
					break;
				case TmdbSerie serie:
					dto = new ResponseItemDto
					{
						Id = serie.Id,
						ImagePath = serie.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{serie.PosterPath}",
						MediaType = "tv",
						Name = serie.Name,
						Overview = serie.Overview,
						Url = $"tvs/{serie.Id}"
					};
					break;
			}
			return dto;
		}
	}
}
