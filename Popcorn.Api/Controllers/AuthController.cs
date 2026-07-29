using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost]
		[Route("register")]
		public async Task<ActionResult> Register([FromBody] RegisterDto dto)
		{
			var result = await _authService.Register(dto);

			if (result)
				return Ok();
			else
				return BadRequest();
		}

		[HttpPost]
		[Route("login")]
		public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
		{
			var result = await _authService.Login(dto);

			if (result != null)
				return Ok(result);
			else
				return Unauthorized();
		}
	}
}
