using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;

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
		public async Task<ActionResult<ResponseDto>> GetMulti([FromQuery] string query, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetMulti(query, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound("No media found");

		}

		[HttpGet]
		[Route("movie")]
		public async Task<ActionResult<ResponseDto>> GetMovie([FromQuery] string query, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetMovie(query, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Movie with title {query} not found!");
		}

		[HttpGet]
		[Route("tv")]
		public async Task<ActionResult<ResponseDto>> GetSerie([FromQuery] string query, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetSerie(query, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Tv serie with title {query} not found!");
		}

		[HttpGet]
		[Route("person")]
		public async Task<ActionResult<TmdbResponse<TmdbPerson>>> GetPerson([FromQuery] string query, [FromQuery] int page = 1)
		{
			var result = await _serachService.GetPerson(query, page);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Person of name {query} not found!");
		}
	}
}
