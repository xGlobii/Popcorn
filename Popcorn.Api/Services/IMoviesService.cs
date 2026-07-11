using Popcorn.Api.Models;

namespace Popcorn.Api.Services
{
	public interface IMoviesService
	{
		public Task<TmdbMovieDetails?> GetMovieDetails(int id);
	}
}
