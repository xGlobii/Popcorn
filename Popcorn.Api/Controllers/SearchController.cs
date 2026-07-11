using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Api.Services;
using System.Text.Json;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/search")]
	public class SearchController : ControllerBase
	{
		private readonly ISearchService _serachService;

		public SearchController(ISearchService searchService)
		{
			_serachService = searchService;
		}

		[HttpGet]
		[Route("multi")]
		public async Task<ActionResult<TmdbResponse<TmdbResult>>> GetMulti([FromQuery] string query, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetMulti(query, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound("No media found");

		}

		[HttpGet]
		[Route("movie")]
		public async Task<ActionResult<TmdbResponse<TmdbMovie>>> GetMovie([FromQuery] string title, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetMovie(title, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Movie with title {title} not found!");
		}

		[HttpGet]
		[Route("tv")]
		public async Task<ActionResult<TmdbResponse<TmdbSerie>>> GetSerie([FromQuery] string title, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetSerie(title, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Tv serie with title {title} not found!");
		}

		[HttpGet]
		[Route("person")]
		public async Task<ActionResult<TmdbResponse<TmdbPerson>>> GetPerson([FromQuery] string name, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetPerson(name, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Person of name {name} not found!");
		}
	}
}
