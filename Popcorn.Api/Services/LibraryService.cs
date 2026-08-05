using Microsoft.EntityFrameworkCore;
using Popcorn.Api.Data;
using Popcorn.Shared.Dto;
using Popcorn.Shared.Enums;
using Popcorn.Api.Models.Entities;

namespace Popcorn.Api.Services
{
	public class LibraryService : ILibraryService
	{
		private readonly AppDbContext _dbContext;

		public LibraryService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<LibraryDto>?> GetLibraryItemsAsync(Guid userId)
		{
			var results = await _dbContext.Activities.Include(a => a.MediaItem).Where(a => a.UserId == userId).ToListAsync();

			if (results == null)
				return null;

			List<LibraryDto> dto = new();

			foreach(var item in results)
			{
				dto.Add(new LibraryDto
				{
					MediaType = item.MediaType,
					Status = item.Status switch
					{ 
						Status.Watched => ActivityStatus.Watched,
						Status.ToWatch => ActivityStatus.ToWatch,
						_ => throw new NotImplementedException()
					},
					AddedAt = item.AddedAt,
					Title = item.MediaItem?.Title ?? "",
					TmdbId = item.TmdbId
				});
			}

			return dto;
		}
	}
}
