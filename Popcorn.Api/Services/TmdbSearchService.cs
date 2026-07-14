using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using System.Net;
using System.Text.Json;

namespace Popcorn.Api.Services
{
	public class TmdbSearchService : ISearchService
	{
		private readonly IHttpClientFactory _factory;
		private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions();

		public TmdbSearchService(IHttpClientFactory factory)
		{
			_factory = factory;

			_jsonOptions.AllowOutOfOrderMetadataProperties = true;
		}

		public async Task<ResponseDto?> GetMulti(string query, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/multi?query={Uri.EscapeDataString(query)}&page={page}");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!response.IsSuccessStatusCode)
				response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbResult>>(_jsonOptions);

			if (result == null)
				return null;

			List<ResponseItemDto> items = new();

			foreach(var item in result.Results)
			{
				switch (item)
				{
					case TmdbMovie movie:
						items.Add(new ResponseItemDto
						{
							Id = movie.Id,
							ImagePath = movie.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}",
							MediaType = "movie",
							Name = movie.Title,
							Overview = movie.Overview,
							Url = $"movies/{movie.Id}"
						});
						break;
					case TmdbSerie serie:
						items.Add(new ResponseItemDto
						{
							Id = serie.Id,
							ImagePath = serie.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{serie.PosterPath}",
							MediaType = "tv",
							Name = serie.Name,
							Overview = serie.Overview,
							Url = $"tvs/{serie.Id}"
						});
						break;
					case TmdbPerson person:
						items.Add(new ResponseItemDto
						{
							Id = person.Id,
							ImagePath = person.ProfilePath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{person.ProfilePath}",
							MediaType = "person",
							Name = person.Name,
							Overview = "",
							Url = $"people/{person.Id}"
						});
						break;
				}
			}

			ResponseDto dto = new ResponseDto
			{
				Page = page,
				Results = items,
				TotalPages = result.TotalPages,
				TotalResults = result.TotalResults
			};

			return dto;
		}

		public async Task<ResponseDto?> GetMovie(string title, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/movie?query={Uri.EscapeDataString(title)}&page={page}");

			if (response.StatusCode == HttpStatusCode.NotFound)
				return null;

			if (!response.IsSuccessStatusCode)
				response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbMovie>>(_jsonOptions);

			if (result == null)
				return null;

			List<ResponseItemDto> items = new();

			foreach(var item in result.Results)
			{
				items.Add(new ResponseItemDto
				{
					Id = item.Id,
					ImagePath = item.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{item.PosterPath}",
					MediaType = "movie",
					Name = item.Title,
					Overview = item.Overview,
					Url = $"movies/{item.Id}"
				});
			}

			ResponseDto dto = new ResponseDto
			{
				Page = page,
				Results = items,
				TotalPages = result.TotalPages,
				TotalResults = result.TotalResults
			};

			return dto;
		}

		public async Task<ResponseDto?> GetSerie(string title, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/tv?query={Uri.EscapeDataString(title)}&page={page}");

			if (response.StatusCode == HttpStatusCode.NotFound)
				return null;

			if (!response.IsSuccessStatusCode)
				response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbSerie>>(_jsonOptions);

			if (result == null)
				return null;

			List<ResponseItemDto> items = new();

			foreach(var item in result.Results)
			{
				items.Add(new ResponseItemDto
				{
					Id = item.Id,
					ImagePath = item.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{item.PosterPath}",
					MediaType = "tv",
					Name = item.Name,
					Overview = item.Overview,
					Url = $"tvs/{item.Id}"
				});
			}

			ResponseDto dto = new ResponseDto
			{
				Page = page,
				Results = items,
				TotalPages = result.TotalPages,
				TotalResults = result.TotalResults
			};

			return dto;
		}

		public async Task<ResponseDto?> GetPerson(string name, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/person?query={Uri.EscapeDataString(name)}&page={page}");

			if (response.StatusCode == HttpStatusCode.NotFound)
				return null;

			if (!response.IsSuccessStatusCode)
				response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbPerson>>(_jsonOptions);

			if (result == null)
				return null;

			List<ResponseItemDto> items = new();

			foreach(var item in result.Results)
			{
				items.Add(new ResponseItemDto
				{
					Id = item.Id,
					ImagePath = item.ProfilePath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{item.ProfilePath}",
					MediaType = "person",
					Name = item.Name,
					Overview = "",
					Url = $"people/{item.Id}"
				});
			}

			ResponseDto dto = new ResponseDto
			{
				Page = page,
				Results = items,
				TotalPages = result.TotalPages,
				TotalResults = result.TotalResults
			};

			return dto;
		}
	}
}
