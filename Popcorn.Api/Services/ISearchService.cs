using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface ISearchService
	{
		Task<ResponseDto?> GetMulti(string query, int page = 1);
		Task<ResponseDto?> GetMovie(string title, int page = 1);
		Task<ResponseDto?> GetSerie(string title, int page = 1);
		Task<ResponseDto?> GetPerson(string name, int page = 1);
	}
}
