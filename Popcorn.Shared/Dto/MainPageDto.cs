namespace Popcorn.Shared.Dto
{
	public class MainPageDto
	{
		public ResponseItemDto Discover { get; set; } = new();
		public required IReadOnlyList<ResponseItemDto> NowPlaying { get; set; }
	}
}
