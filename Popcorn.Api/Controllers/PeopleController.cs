using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Dto;
using Popcorn.Api.Services;

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
	}
}
