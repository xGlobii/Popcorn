using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Popcorn.Client.Services
{
	public class TokenAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly IJSRuntime _jsRuntime;

		public TokenAuthenticationStateProvider(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");

			if(token == null)
			{
				return new AuthenticationState(new ClaimsPrincipal());
			}

			string[] parts = token.Split('.');
			string payload = parts[1];
			string base64 = payload.Replace('-', '+').Replace('_', '/');

			switch(base64.Length % 4)
			{
				case 2:
					base64 += "==";
					break;
				case 3:
					base64 += "=";
					break;
			}

			var payloadString = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
			Dictionary<string, object>? claims = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadString);

			if(claims == null)
			{
				return new AuthenticationState(new ClaimsPrincipal());
			}

			List<Claim> claimList = new();

			foreach(var claim in claims)
			{
				claimList.Add(new Claim(claim.Key, claim.Value.ToString() ?? ""));
			}

			var identity = new ClaimsIdentity(claimList, "Popcorn Authentication");

			var user = new ClaimsPrincipal(identity);

			return new AuthenticationState(user);
		}

		public void UpdateAuthState()
		{
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
