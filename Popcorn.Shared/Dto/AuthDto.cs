using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Popcorn.Shared.Dto
{
	public class RegisterDto
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string Username { get; set; } = string.Empty;
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
		[Required]
		public string Password { get; set; } = string.Empty;
		[Compare("Password", ErrorMessage = "Passwords must be identical")]
		[Required]
		public string ConfirmPassword { get; set; } = string.Empty;
	}

	public class LoginDto
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;
	}

	public class AuthTokensDto
	{
		public string Token { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = string.Empty;
	}
}
