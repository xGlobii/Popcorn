using Microsoft.EntityFrameworkCore;
using Popcorn.Api.Data;
using Popcorn.Api.Models;
using Popcorn.Shared.Dto;
using Popcorn.Shared.Enums;
using System.Net;
using Popcorn.Api.Models.Entities;

namespace Popcorn.Api.Services
{
	public class TmdbTvSeriesService : ITvSeriesService
	{
		private readonly IHttpClientFactory _factory;
		private readonly AppDbContext _dbContext;

		public TmdbTvSeriesService(IHttpClientFactory factory, AppDbContext dbContext)
		{
			_factory = factory;
			_dbContext = dbContext;
		}

		public async Task<TvSerieDetailsDto?> GetTvSerieDetails(int id, Guid userId)
		{
			var client = _factory.CreateClient("TMDB");
			var response = await client.GetAsync($"tv/{id}?append_to_response=aggregate_credits");

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!response.IsSuccessStatusCode)
			{
				response.EnsureSuccessStatusCode();
			}

			var result = await response.Content.ReadFromJsonAsync<TmdbTvSerieDetails>();

			if (result == null)
				return null;

			List<SeasonDto> seasons = new();

			foreach (var season in result.Seasons)
			{
				if (season.SeasonNumber != 0)
				{
					seasons.Add(new SeasonDto
					{
						Name = season.Name,
						Overview = season.Overview == null ? "" : season.Overview,
						SeasonNumber = season.SeasonNumber
					});
				}
			}

			List<PersonDto> cast = new();

			foreach (var person in result.Credits.Cast.Take(15))
			{
				if (person.Department == "Acting")
				{
					cast.Add(new PersonDto
					{
						Id = person.Id,
						Name = person.Name,
						ProfilePath = person.ProfilePath == null ? "" : $"https://image.tmdb.org/t/p/w500{person.ProfilePath}",
						Character = person.Roles[0].Character
					});
				}
			}

			ActivityStatus status = ActivityStatus.None;

			if (userId != Guid.Empty)
			{
				var media = await _dbContext.Activities.FirstOrDefaultAsync(a => a.UserId == userId && a.MediaType == "tv" && a.TmdbId == id);

				if(media != null)
				{
					status = media.Status switch
					{
						Status.Watched => ActivityStatus.Watched,
						Status.ToWatch => ActivityStatus.ToWatch,
						_ => throw new NotImplementedException()
					};
				}
			}

			return new TvSerieDetailsDto
			{
				Id = result.Id,
				Name = result.Name,
				FirstAirDate = result.FirstAirDate,
				NumberOfEpisodes = result.NumberOfEpisodes,
				NumberOfSeasons = result.NumberOfSeasons,
				Overview = result.Overview,
				Status = result.Status,
				PosterPath = result.PosterPath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{result.PosterPath}",
				HomePage = result.HomePage == null ? null : result.HomePage,
				Seasons = seasons,
				Cast = cast,
				MediaStatus = status
			};
		}

		public async Task<EpisodesDetailsDto?> GetEpisodesDetails(int id, int seasonNumber)
		{
			var client = _factory.CreateClient("TMDB");
			var resposne = await client.GetAsync($"tv/{id}/season/{seasonNumber}");

			if (resposne.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			if (!resposne.IsSuccessStatusCode)
			{
				resposne.EnsureSuccessStatusCode();
			}

			var result = await resposne.Content.ReadFromJsonAsync<EpisodesDetails>();

			if (result == null)
				return null;

			List<EpisodeDto> episodes = new();

			foreach (var episode in result.Episodes)
			{
				List<PersonDto> guests = new();

				foreach (var guest in episode.GuestStars)
				{
					guests.Add(new PersonDto
					{
						Character = guest.Character,
						Id = guest.Id,
						Name = guest.Name,
						ProfilePath = guest.ProfilePath == null ? "placeholder" : $"https://image.tmdb.org/t/p/w500{guest.ProfilePath}"
					});
				}

				episodes.Add(new EpisodeDto
				{
					Id = episode.Id,
					ImagePath = $"https://image.tmdb.org/t/p/w500{episode.ImagePath}",
					Name = episode.Name,
					Overview = episode.Overview == null ? "" : episode.Overview,
					EpisodeNumber = episode.EpisodeNumber,
					GuestStars = guests
				});
			}

			return new EpisodesDetailsDto
			{
				Episodes = episodes
			};
		}
	}
}
