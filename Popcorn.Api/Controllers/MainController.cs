using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1")]
	public class MainController : ControllerBase
	{
		private readonly IMainService _mainService;

		public MainController(IMainService mainService)
		{
			_mainService = mainService;
		}

		[HttpGet]
		[Route("main")]
		public async Task<ActionResult<MainPageDto>> GetMainPage()
		{
			var result = await _mainService.GetMainPage();

			if (result != null)
				return Ok(result);
			else
				return BadRequest();
		}
	}
}
