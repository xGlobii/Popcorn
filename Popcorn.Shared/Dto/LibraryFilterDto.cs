using Popcorn.Shared.Enums;

namespace Popcorn.Shared.Dto
{
	public class LibraryFilterDto
	{
		public int Page { get; set; } = 1;
		public string? Query { get; set; }
		public string? MediaType { get; set; }
		public string? Status { get; set; }
	}
}
