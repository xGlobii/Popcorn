using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;
using Popcorn.Api.Services;
using Popcorn.Shared.Dto;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/people")]
	public class PeopleController : ControllerBase
	{
		private readonly IPeopleService _peopleService;

		public PeopleController(IPeopleService peopleService)
		{
			_peopleService = peopleService;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<PersonDetailsDto>> Get([FromRoute] int id)
		{
			var result = await _peopleService.GetPersonDetails(id);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Person with id {id} not found!");
		}

		[HttpGet]
		[Route("{id}/filmography-preview")]
		public async Task<ActionResult<List<CreditsDto>>> GetFilmographyPreview([FromRoute] int id)
		{
			var result = await _peopleService.GetFilmographyPreview(id);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Person with id {id} not found!");
		}

		[HttpGet]
		[Route("{id}/filmography")]
		public async Task<ActionResult<CombinedCreditsDto>> GetFilmography([FromRoute] int id)
		{
			var result = await _peopleService.GetFilmography(id);

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Person with id {id} not found!");
		}
	}
}
