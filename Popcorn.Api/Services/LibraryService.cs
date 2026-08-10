using Microsoft.EntityFrameworkCore;
using Popcorn.Api.Data;
using Popcorn.Api.Models.Entities;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public class LibraryService : ILibraryService
	{
		private readonly AppDbContext _dbContext;
		private const int PAGE_SIZE = 20;

		public LibraryService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<LibraryDto?> GetLibraryItemsAsync(Guid userId, LibraryFilterDto dto)
		{
			if (!VerifyDto(dto))
				return null;

			var results = _dbContext.Activities.Include(a => a.MediaItem).Where(a => a.UserId == userId);

			if (!string.IsNullOrEmpty(dto.Query))
				results = results.Where(a => a.MediaItem != null && a.MediaItem.Title.Contains(dto.Query));

			if (!string.IsNullOrEmpty(dto.MediaType))
				results = results.Where(a => a.MediaType == dto.MediaType);
			if (!string.IsNullOrWhiteSpace(dto.Status))
			{
				Enum.TryParse(dto.Status, ignoreCase: true, out Status status);
				results = results.Where(a => a.Status == status);
			}

			var pagedResults = await results.Skip((dto.Page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToListAsync();

			List<MediaItemDto> newDto = new();

			foreach(var item in pagedResults)
			{
				newDto.Add(new MediaItemDto
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
					PosterPath = item.MediaItem?.PosterPath ?? ""
				});
			}

			return new LibraryDto
			{
				Items = newDto,
				TotalPages = (await results.CountAsync() + PAGE_SIZE - 1) / PAGE_SIZE
			};
		}

		private bool VerifyDto(LibraryFilterDto dto)
		{
			bool status = true;
			bool mediaType = true;
			bool page = true;

			page = dto.Page > 0;

			if(!string.IsNullOrEmpty(dto.Status))
			{
				dto.Status = dto.Status.Replace("_", "");
				status = Enum.TryParse(dto.Status, ignoreCase: true, out Status _);
			}
			if (!string.IsNullOrWhiteSpace(dto.MediaType))
			{
				dto.MediaType = dto.MediaType.ToLower();
				mediaType = dto.MediaType == "movie" || dto.MediaType.ToLower() == "tv";
			}

			return status && mediaType && page;
		}
	}
}
