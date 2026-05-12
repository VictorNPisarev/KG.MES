using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class OrderSupplyDto
	{
		[JsonPropertyName("order_supply_id")]
		public string OrderSupplyId { get; set; } = string.Empty;

		[JsonPropertyName("supply_type_id")]
		public string SupplyTypeId { get; set; } = string.Empty;

		[JsonPropertyName("supply_condition_id")]
		public string? SupplyConditionId { get; set; }

		[JsonPropertyName("expected_date")]
		public DateTime? ExpectedDate { get; set; }

		[JsonPropertyName("quantity")]
		public double? Quantity { get; set; }

		[JsonPropertyName("comment")]
		public string? Comment { get; set; }

	}
}