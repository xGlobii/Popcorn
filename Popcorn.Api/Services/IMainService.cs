using Popcorn.Api.Models;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Services
{
	public interface IMainService
	{
		public Task<MainPageDto?> GetMainPage();
	}
}
