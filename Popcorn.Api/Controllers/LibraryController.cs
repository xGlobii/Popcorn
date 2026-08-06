using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;
using System.Security.Claims;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/library")]
	public class LibraryController : ControllerBase
	{
		private readonly ILibraryService _service;

		public LibraryController(ILibraryService service)
		{
			_service = service;
		}

		[Authorize]
		[HttpGet]
		public async Task<ActionResult<LibraryDto>?> GetList([FromQuery] int page = 1, [FromQuery] string query = "")
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);

			Console.WriteLine(page);

			var result = await _service.GetLibraryItemsAsync(userId, page, query);

			if (result != null)
				return Ok(result);
			else
				return Ok(new LibraryDto
				{ 
					Items = new List<LibraryItemDto>()
				});
		}
	}
}
