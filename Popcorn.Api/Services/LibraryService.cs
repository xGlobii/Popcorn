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
		private const int PAGE_SIZE = 4;

		public LibraryService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<LibraryDto?> GetLibraryItemsAsync(Guid userId, int page, string query)
		{
			var results = _dbContext.Activities.Include(a => a.MediaItem).Where(a => a.UserId == userId);

			if (!string.IsNullOrEmpty(query))
				results = results.Where(a => a.MediaItem != null && a.MediaItem.Title.Contains(query));

			var pagedResults = await results.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToListAsync();

			List<LibraryItemDto> dto = new();

			foreach(var item in pagedResults)
			{
				dto.Add(new LibraryItemDto
				{
					MediaType = item.MediaType,
					Status = item.Status switch
					{
						Status.Watched => "Watched",
						Status.ToWatch => "To watch",
						_ => throw new NotImplementedException()
					},
					AddedAt = item.AddedAt,
					Title = item.MediaItem?.Title ?? "",
					TmdbId = item.TmdbId,
				});
			}

			return new LibraryDto
			{
				Items = dto,
				TotalPages = (await results.CountAsync() + PAGE_SIZE - 1) / PAGE_SIZE
			};
		}
	}
}
