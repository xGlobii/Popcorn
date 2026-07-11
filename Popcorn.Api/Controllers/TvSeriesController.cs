using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Dto;
using Popcorn.Api.Services;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/tvs")]
	public class TvSeriesController : ControllerBase
	{
		private readonly ITvSeriesService _tvSeriesService;

		public TvSeriesController(ITvSeriesService tvSeriesService)
		{
			_tvSeriesService = tvSeriesService;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<TvSerieDetailsDto>> Get([FromRoute] int id)
		{
			var result = await _tvSeriesService.GetTvSerieDetails(id);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Tv serie with id {id} not found!");
		}
	}
}
