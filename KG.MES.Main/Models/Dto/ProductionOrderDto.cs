using System.Text.Json.Serialization;

namespace XmlOrderReader.Web.Models.Dto
{
	public class ProductionOrderExportDto
	{
		// Автоматически из XML
		public string OrderNumber { get; set; } = string.Empty;
		public int WindowCount { get; set; }
		public double WindowArea { get; set; }
		public int PlateCount { get; set; }
		public double PlateArea { get; set; }
		public string? Lumber { get; set; }
		public string? GlazingBead { get; set; }
		public bool IsTwoSidePaint { get; set; }

		// Редактируемые поля
		public bool IsEconom { get; set; } = false;
		public bool IsClaim { get; set; } = false;
		public bool IsOnlyPaid { get; set; } = false;
		public string? Comment { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Now;  // "2026-04-21"
		public int ProductionDays { get; set; } = 60;          // срок изготовления в днях
		public DateTime? ReadyDate { get; set; }  // вычисляется на сервере или клиенте
	}

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