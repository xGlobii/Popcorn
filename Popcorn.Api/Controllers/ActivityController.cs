using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;
using System.Security.Claims;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/activity")]
	public class ActivityController : ControllerBase
	{
		private readonly IActivityService _service;

		public ActivityController(IActivityService service)
		{
			_service = service;
		}

		[Authorize]
		[HttpPut]
		public async Task<ActionResult> Add([FromBody] ActivityDto dto)
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);

			var result = await _service.UpsertAsync(dto, userId);
			if (result)
				return Ok();
			else
				return BadRequest();
		}

		[Authorize]
		[HttpDelete]
		[Route("{mediaType}/{id}")]
		public async Task<ActionResult> Delete([FromRoute] string mediaType, [FromRoute] int id)
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);

			var result = await _service.DeleteAsync(mediaType, id, userId);
			if (result)
				return Ok();
			else
				return BadRequest();
		}
	}
}
