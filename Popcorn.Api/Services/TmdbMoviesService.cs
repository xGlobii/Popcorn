using Popcorn.Api.Data;
using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using System.Net;
using Popcorn.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Popcorn.Api.Services
{
	public class TmdbMoviesService : IMoviesService
	{
		private readonly IHttpClientFactory _factory;
		private readonly AppDbContext _dbContext;

		public TmdbMoviesService(IHttpClientFactory factory, AppDbContext dbContext)
		{
			_factory = factory;
			_dbContext = dbContext;
		}

		public async Task<MovieDetailsDto?> GetMovieDetails(int id, Guid userId)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"movie/{id}?append_to_response=credits");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!response.IsSuccessStatusCode)
			{
				response.EnsureSuccessStatusCode();
			}

			var result = await response.Content.ReadFromJsonAsync<TmdbMovieDetails>();

			if (result == null)
				return null;

			List<PersonDto> cast = new();

			foreach (var person in result.Credits.Cast.Take(15))
			{
				if (person.Department == "Acting")
				{
					cast.Add(new PersonDto
					{
						Id = person.Id,
						Name = person.Name,
						ProfilePath = person.ProfilePath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{person.ProfilePath}",
						Character = person.Character
					});
				}
			}

			ActivityStatus status = ActivityStatus.None;

			if (userId != Guid.Empty)
			{
				var media = await _dbContext.Activities.FirstOrDefaultAsync(a => a.UserId == userId && a.TmdbId == id && a.MediaType == "movie");

				if (media != null)
				{
					status = media.Status switch
					{
						Models.Entities.Status.Watched => ActivityStatus.Watched,
						Models.Entities.Status.ToWatch => ActivityStatus.ToWatch,
						_ => throw new NotImplementedException()
					};
				}
			}

			return new MovieDetailsDto
			{
				Id = result.Id,
				HomePage = result.HomePage == null ? null : result.HomePage,
				Overview = result.Overview,
				PosterPath = result.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{result.PosterPath}",
				ReleaseDate = result.ReleaseDate,
				Status = result.Status,
				Title = result.Title,
				Cast = cast,
				MediaStatus = status
			};
		}
	}
}
