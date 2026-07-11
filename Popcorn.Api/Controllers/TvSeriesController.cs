using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Api.Services;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/tvs")]
	public class TvSeriesController : ControllerBase
	{
		private readonly IHttpClientFactory _factory;
		private readonly ITvSeriesService _tvSeriesService;

		public TvSeriesController(IHttpClientFactory factory, ITvSeriesService tvSeriesService)
		{
			_factory = factory;
			_tvSeriesService = tvSeriesService;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<TmdbSerieDetails>> Get([FromRoute] int id)
		{
			//var client = _factory.CreateClient("TMDB");
			//var respond = await client.GetAsync($"tv/{id}");

			//respond.EnsureSuccessStatusCode();

			//var result = await respond.Content.ReadFromJsonAsync<TmdbSerieDetails>();

			var result = await _tvSeriesService.GetTvSerieDetails(id);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Tv serie with id {id} not found!");
		}
	}
}
