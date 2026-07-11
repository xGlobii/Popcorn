using Microsoft.AspNetCore.Mvc;
using Popcorn.Api.Models;

namespace Popcorn.Api.Controllers
{
	[ApiController]
	[Route("api/v1/people")]
	public class PeopleController : ControllerBase
	{
		private readonly IHttpClientFactory _factory;

		public PeopleController(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<TmdbPersonDetails>> Get([FromRoute] int id)
		{
			var client = _factory.CreateClient("TMDB");
			var respond = await client.GetAsync($"person/{id}");

			respond.EnsureSuccessStatusCode();

			var result = await respond.Content.ReadFromJsonAsync<TmdbPersonDetails>();

			if (result != null)
				return Ok(result);
			else
				return NotFound($"Person with id {id} not found!");
		}
	}
}
