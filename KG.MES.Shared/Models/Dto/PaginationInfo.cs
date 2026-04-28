using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class PaginationInfo
	{
		[JsonPropertyName("page")]
		public int Page { get; set; }

		[JsonPropertyName("limit")]
		public int Limit { get; set; }

		[JsonPropertyName("total")]
		public int Total { get; set; }

		[JsonPropertyName("pages")]
		public int Pages { get; set; }
	}
}