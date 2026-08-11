using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;
using System.Security.Claims;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/profile")]
	public class ProfileController : ControllerBase
	{
		private readonly IProfileService _service;

		public ProfileController(IProfileService service)
		{
			_service = service;
		}

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<ProfileDto>> GetDetails()
		{
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
			var username = User.FindFirstValue(ClaimTypes.Name);

			if (username == null)
				return Ok(new ProfileDto());

			var result = await _service.GetDetailsAsync(userId);

			if (result != null)
			{
				result.Username = username;
				return Ok(result);
			}
			else
				return Ok(new ProfileDto());
		}
	}
}
