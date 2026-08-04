using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;
using System.Security.Claims;

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
			Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
			var result = await _tvSeriesService.GetTvSerieDetails(id, userId);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Tv serie with id {id} not found!");
		}

		[HttpGet]
		[Route("{id}/season/{seasonNumber}")]
		public async Task<ActionResult<EpisodesDetailsDto>> Get([FromRoute] int id, [FromRoute] int seasonNumber)
		{
			var result = await _tvSeriesService.GetEpisodesDetails(id, seasonNumber);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Tv serie with id {id} or season {seasonNumber} not found!");
		}
	}
}
