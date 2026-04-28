using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class PaginatedResponse<T>
	{
		[JsonPropertyName("data")]
		public List<T> Data { get; set; } = new();

		[JsonPropertyName("pagination")]
		public PaginationInfo Pagination { get; set; } = new();

		// Удобные свойства для UI
		public int Page => Pagination.Page;
		public int Limit => Pagination.Limit;
		public int Total => Pagination.Total;
		public int TotalPages => Pagination.Pages;
		public bool HasNextPage => Page < TotalPages;
		public bool HasPreviousPage => Page > 1;
	}
}