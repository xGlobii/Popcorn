using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IMoviesService
	{
		public Task<MovieDetailsDto?> GetMovieDetails(int id);
	}
}
