using Microsoft.JSInterop;
using Popcorn.Shared.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Popcorn.Client.HttpHandlers
{
	public class TokenDelegationHandler : DelegatingHandler
	{
		private readonly IJSRuntime _js;
		private readonly List<string> _blackList = new List<string>()
		{
			"/api/v1/auth/login",
			"/api/v1/auth/register",
			"/api/v1/auth/refresh"
		};

		public TokenDelegationHandler(IJSRuntime js)
		{
			_js = js;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _js.InvokeAsync<string>("localStorage.getItem", "accessToken");

			HttpResponseMessage response;

			if (string.IsNullOrEmpty(token))
			{
				response = await base.SendAsync(request, cancellationToken);
			}
			else
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				response = await base.SendAsync(request, cancellationToken);
			}

			if (request.RequestUri == null)
				return response;

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !_blackList.Any(l => l.Equals(request.RequestUri.AbsolutePath, StringComparison.OrdinalIgnoreCase)))
			{
				var refreshToken = await _js.InvokeAsync<string>("localStorage.getItem", "refreshToken");

				using HttpClient client = new HttpClient
				{
					BaseAddress = new Uri(request.RequestUri.GetLeftPart(UriPartial.Authority))
				};
				var tokenResponse = await client.PostAsJsonAsync<AuthTokensDto>("api/v1/auth/refresh", new AuthTokensDto
				{
					RefreshToken = refreshToken,
					Token = token
				});

				if (tokenResponse.IsSuccessStatusCode)
				{
					AuthTokensDto? newTokens = await tokenResponse.Content.ReadFromJsonAsync<AuthTokensDto>();

					if (newTokens == null)
					{
						await _js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
						await _js.InvokeVoidAsync("localStorage.removeItem", "accessToken");
						return response;
					}

					await _js.InvokeVoidAsync("localStorage.setItem", "refreshToken", newTokens.RefreshToken);
					await _js.InvokeVoidAsync("localStorage.setItem", "accessToken", newTokens.Token);

					HttpRequestMessage newMessage = new HttpRequestMessage
					{
						Method = request.Method,
						RequestUri = request.RequestUri,
					};

					foreach (var header in request.Headers)
					{
						newMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
					}

					if (request.Content != null)
					{
						var newContent = await request.Content.ReadAsByteArrayAsync();
						ByteArrayContent content = new ByteArrayContent(newContent);

						newMessage.Content = content;

						foreach (var contentHeader in request.Content.Headers)
						{
							newMessage.Content.Headers.TryAddWithoutValidation(contentHeader.Key, contentHeader.Value);
						}
					}

					newMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.Token);
					response = await base.SendAsync(newMessage, cancellationToken);
				}
				else
				{
					await _js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
					await _js.InvokeVoidAsync("localStorage.removeItem", "accessToken");
				}
			}

			return response;
		}
	}
}
