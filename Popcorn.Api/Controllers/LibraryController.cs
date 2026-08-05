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
		public async Task<ActionResult<IReadOnlyList<LibraryDto>?>> GetList()
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);

			var result = await _service.GetLibraryItemsAsync(userId);

			if (result != null)
				return Ok(result);
			else
				return Ok(new List<LibraryDto>());
		}
	}
}
