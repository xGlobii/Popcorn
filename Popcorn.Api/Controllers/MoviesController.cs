using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Api.Services;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/movies")]
	public class MoviesController : ControllerBase
	{
		private readonly IMoviesService _moviesService;

		public MoviesController(IMoviesService moviesService)
		{
			_moviesService = moviesService;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<TmdbMovieDetails>> Get([FromRoute] int id)
		{
			var result = await _moviesService.GetMovieDetails(id);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Movie with id {id} not found!");

		}
	}
}
