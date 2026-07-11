using Popcorn.Api.Dto;

namespace Popcorn.Api.Services
{
	public interface IPeopleService
	{
		public Task<PersonDetailsDto?> GetPersonDetails(int id);
	}
}
