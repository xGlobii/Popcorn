
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Popcorn.Api.Data;
using Popcorn.Api.Services;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace Popcorn.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var corsPolicy = "PopcornApiPolicy";

			var key = builder.Configuration["Auth:SecurityKey"];

			if (key == null)
				throw new ArgumentNullException("Security key is not set");

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

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
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IActivityService, ActivityService>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = "Popcorn",
					ValidateAudience = true,
					ValidAudience = "Popcorn.Api",
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = securityKey,
					ClockSkew = TimeSpan.Zero
				};
			});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy(name: corsPolicy, policy =>
				{
					policy.WithOrigins("https://localhost:7224").AllowAnyMethod().AllowAnyHeader();
				});
			});

			builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(builder.Configuration["ConnectionStrings:DbConnectionString"]));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
			}

			app.UseHttpsRedirection();

			app.UseCors(corsPolicy);

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
