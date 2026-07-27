
using Microsoft.Net.Http.Headers;
using Popcorn.Api.Services;
using Scalar.AspNetCore;
using System.Net.Http.Headers;

namespace Popcorn.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var corsPolicy = "PopcornApiPolicy";

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();
			builder.Services.AddRouting(options => options.LowercaseUrls = true);

			builder.Services.AddHttpClient("TMDB", options =>
			{
				options.BaseAddress = new Uri("https://api.themoviedb.org/3/");
				options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", builder.Configuration["TmdbApiKey"]);
			});

			builder.Services.AddScoped<ISearchService, TmdbSearchService>();
			builder.Services.AddScoped<IMoviesService, TmdbMoviesService>();
			builder.Services.AddScoped<ITvSeriesService, TmdbTvSeriesService>();
			builder.Services.AddScoped<IPeopleService, TmdbPeopleService>();
			builder.Services.AddScoped<IMainService, TmdbMainService>();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy(name: corsPolicy, policy =>
				{
					policy.WithOrigins("https://localhost:7224");
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
			}

			app.UseHttpsRedirection();

			app.UseCors(corsPolicy);

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
