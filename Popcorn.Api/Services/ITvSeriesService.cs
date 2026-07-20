using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface ITvSeriesService
	{
		public Task<TvSerieDetailsDto?> GetTvSerieDetails(int id);
		public Task<EpisodesDetailsDto?> GetEpisodesDetails(int id, int seasonNumber);
	}
}
