using Popcorn.Api.Dto;

namespace Popcorn.Api.Services
{
	public interface ITvSeriesService
	{
		public Task<TvSerieDetailsDto?> GetTvSerieDetails(int id);
	}
}
