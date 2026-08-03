using Microsoft.EntityFrameworkCore;
using Popcorn.Api.Data;
using Popcorn.Shared.Enums;
using Popcorn.Api.Models.Entities;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public class ActivityService : IActivityService
	{
		private readonly AppDbContext _dbContext;

		public ActivityService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> UpsertAsync(ActivityDto dto, Guid userId)
		{
			try
			{
				var record = await _dbContext.Activities.FirstOrDefaultAsync(a => a.MediaType == dto.MediaType && a.TmdbId == dto.TmdbId && a.UserId == userId);

				Status recordStatus = dto.Status switch
				{
					ActivityStatus.ToWatch => Status.ToWatch,
					ActivityStatus.Watched => Status.Watched,
					ActivityStatus.None => throw new NotImplementedException(),
					_ => throw new NotImplementedException()
				};

				var mediaItem = await _dbContext.MediaItem.FirstOrDefaultAsync(a => a.MediaType == dto.MediaType && a.TmdbId == dto.TmdbId);

				if(mediaItem == null)
				{
					mediaItem = new TmdbMedia
					{
						MediaType = dto.MediaType,
						Title = dto.Title,
						TmdbId = dto.TmdbId
					};
				}

				if (record == null)
				{
					await _dbContext.Activities.AddAsync(new ActivityItem
					{
						MediaType = dto.MediaType,
						Status = recordStatus,
						TmdbId = dto.TmdbId,
						UserId = userId,
						MediaItem = mediaItem
					});
				}
				else
				{
					record.Status = recordStatus;
				}

				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> DeleteAsync(string mediaType, int id, Guid userId)
		{
			try
			{
				var result = await _dbContext.Activities.Where(a => a.MediaType == mediaType && a.TmdbId == id && a.UserId == userId).ExecuteDeleteAsync();

				return result > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
