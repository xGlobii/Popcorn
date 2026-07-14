namespace Popcorn.Shared.Dto
{
	public class ResponseDto
	{
		public int Page { get; set; }
		public required List<ResponseItemDto> Results { get; set; }
		public int TotalPages { get; set; }
		public int TotalResults { get; set; }
	}

	public class ResponseItemDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Overview { get; set; } = string.Empty;
		public string ImagePath { get; set; } = string.Empty;
		public string MediaType { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
	}
}
