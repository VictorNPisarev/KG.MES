using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class ProductionOrderDto
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("order_number")]
		public string OrderNumber { get; set; } = string.Empty;

		[JsonPropertyName("ready_date")]
		public DateTime? ReadyDate { get; set; }

		[JsonPropertyName("window_count")]
		public int WindowCount { get; set; }

		[JsonPropertyName("window_area")]
		public string? WindowArea { get; set; }

		[JsonPropertyName("plate_count")]
		public int PlateCount { get; set; }

		[JsonPropertyName("plate_area")]
		public string? PlateArea { get; set; }

		[JsonPropertyName("is_econom")]
		public bool IsEconom { get; set; }

		[JsonPropertyName("is_claim")]
		public bool IsClaim { get; set; }

		[JsonPropertyName("is_only_paid")]
		public bool IsOnlyPaid { get; set; }

		[JsonPropertyName("production_order_id")]
		public string? ProductionOrderId { get; set; }

		[JsonPropertyName("current_workplace_id")]
		public string? CurrentWorkplaceId { get; set; }

		[JsonPropertyName("current_status")]
		public string? Status { get; set; }

		[JsonPropertyName("created_at")]
		public DateTime StartDate { get; set; }

		// Поля для отображения (не из API, заполняем сами)
		[JsonIgnore]
		public string CustomerName { get; set; } = string.Empty;

		[JsonIgnore]
		public decimal TotalAmount { get; set; }

		[JsonIgnore]
		public double WindowAreaDouble => double.TryParse(WindowArea?.Replace(".", ","), out var result) ? result : 0;

		[JsonIgnore]
		public double PlateAreaDouble => double.TryParse(PlateArea?.Replace(".", ","), out var result) ? result : 0;
	}

}