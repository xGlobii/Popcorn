using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
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

		public async Task<TmdbResponse<TmdbResult>?> GetMulti(string query, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/multi?query={Uri.EscapeDataString(query)}&page={page}");

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbResult>>(_jsonOptions);

			if (result != null)
				return result;
			else
				return null;
		}

		public async Task<TmdbResponse<TmdbMovie>?> GetMovie(string title, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/movie?query={Uri.EscapeDataString(title)}&page={page}");

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbMovie>>(_jsonOptions);

			if (result != null)
				return result;
			else
				return null;
		}

		public async Task<TmdbResponse<TmdbSerie>?> GetSerie(string title, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/tv?query={Uri.EscapeDataString(title)}&page={page}");

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbSerie>>(_jsonOptions);

			if (result != null)
				return result;
			else
				return null;
		}

		public async Task<TmdbResponse<TmdbPerson>?> GetPerson(string name, int page = 1)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"search/person?query={Uri.EscapeDataString(name)}&page={page}");

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<TmdbResponse<TmdbPerson>>(_jsonOptions);

			if (result != null)
				return result;
			else
				return null;
		}
	}
}
