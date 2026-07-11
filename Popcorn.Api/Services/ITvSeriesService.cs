using Popcorn.Api.Models;

namespace Popcorn.Api.Services
{
	public interface ITvSeriesService
	{
		public Task<TmdbSerieDetails?> GetTvSerieDetails(int id);
	}
}
