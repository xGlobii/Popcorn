using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IPeopleService
	{
		public Task<PersonDetailsDto?> GetPersonDetails(int id);
		public Task<IEnumerable<CreditsDto>?> GetFilmographyPreview(int id);

		public Task<CombinedCreditsDto?> GetFilmography(int id);
	}
}
