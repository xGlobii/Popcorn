using Microsoft.EntityFrameworkCore;
using Popcorn.Api.Data;
using Popcorn.Api.Models.Entities;
using Popcorn.Shared.Dto;
using Popcorn.Shared.Enums;

namespace Popcorn.Api.Services
{
	public class ProfileService : IProfileService
	{
		private readonly AppDbContext _dbContext;

		public ProfileService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ProfileDto?> GetDetailsAsync(Guid userId)
		{
			var moviesCount = await _dbContext.Activities.CountAsync(a => a.UserId == userId && a.MediaType == "movie");
			var tvSeriesCount = await _dbContext.Activities.CountAsync(a => a.UserId == userId && a.MediaType == "tv");
			var watchedCount = await _dbContext.Activities.CountAsync(a => a.UserId == userId && a.Status == Status.Watched);
			var toWatchCount = await _dbContext.Activities.CountAsync(a => a.UserId == userId && a.Status == Status.ToWatch);

			var movies = await _dbContext.Activities.Include(a => a.MediaItem).AsNoTracking().OrderByDescending(a => a.AddedAt).Where(a => a.MediaType == "movie" && a.UserId == userId).Take(3).ToListAsync();
			var series = await _dbContext.Activities.Include(a => a.MediaItem).AsNoTracking().OrderByDescending(a => a.AddedAt).Where(a => a.MediaType == "tv" && a.UserId == userId).Take(3).ToListAsync();

			List<MediaItemDto> latestMovies = new();

			foreach(var movie in movies)
			{
				latestMovies.Add(new MediaItemDto
				{
					AddedAt = movie.AddedAt,
					MediaType = movie.MediaType,
					PosterPath = movie.MediaItem?.PosterPath ?? "",
					Status = movie.Status switch
					{
						Status.ToWatch => "To watch",
						Status.Watched => "Watched",
						_ => throw new NotImplementedException()
					},
					Title = movie.MediaItem?.Title ?? "",
					TmdbId = movie.TmdbId
				});
			}

			List<MediaItemDto> latestTvSeries = new();

			foreach(var serie in series)
			{
				latestTvSeries.Add(new MediaItemDto
				{
					AddedAt = serie.AddedAt,
					MediaType = serie.MediaType,
					PosterPath = serie.MediaItem?.PosterPath ?? "",
					Status = serie.Status switch
					{
						Status.ToWatch => "To watch",
						Status.Watched => "Watched",
						_ => throw new NotImplementedException()
					},
					Title = serie.MediaItem?.Title ?? "",
					TmdbId = serie.TmdbId
				});
			}

			return new ProfileDto
			{
				Movies = latestMovies,
				TvSeries = latestTvSeries,
				MoviesCount = moviesCount,
				ToWatchCount = toWatchCount,
				TvSeriesCount = tvSeriesCount,
				WatchedCount = watchedCount
			};
		}
	}
}
