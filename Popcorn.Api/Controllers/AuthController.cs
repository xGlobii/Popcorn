using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/auth")]
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
		public async Task<ActionResult<AuthTokensDto>> Login([FromBody] LoginDto dto)
		{
			var result = await _authService.Login(dto);

			if (result != null)
				return Ok(result);
			else
				return Unauthorized();
		}

		[HttpPost]
		[Route("refresh")]
		public async Task<ActionResult<AuthTokensDto>> Refresh([FromBody] AuthTokensDto dto)
		{
			var result = await _authService.Refresh(dto);

			if (result != null)
				return Ok(result);
			else
				return Unauthorized();
		}

		[Authorize]
		[HttpPost]
		[Route("logout")]
		public async Task<ActionResult> Logout()
		{
			if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
				return BadRequest();
			if (!Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sid), out Guid sessionId))
				return BadRequest();

			var result = await _authService.Logout(userId, sessionId);

			if (result)
				return Ok();
			else
				return BadRequest();
		}
	}
}
