using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;

namespace Popcorn.Api.Services
{
	public interface ISearchService
	{
		Task<TmdbResponse<TmdbResult>?> GetMulti(string query, int page = 1);
		Task<TmdbResponse<TmdbMovie>?> GetMovie(string title, int page = 1);
		Task<TmdbResponse<TmdbSerie>?> GetSerie(string title, int page = 1);
		Task<TmdbResponse<TmdbPerson>?> GetPerson(string name, int page = 1);
	}
}
