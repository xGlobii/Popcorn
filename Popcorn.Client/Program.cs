using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Popcorn.Client.HttpHandlers;

namespace Popcorn.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddTransient<TokenDelegationHandler>();

			builder.Services.AddHttpClient("Api", options =>
			options.BaseAddress = new Uri("https://localhost:7177"))
				.AddHttpMessageHandler<TokenDelegationHandler>();

			builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

			await builder.Build().RunAsync();
		}
	}
}
