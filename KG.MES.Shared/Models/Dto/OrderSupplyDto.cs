using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class OrderSupplyDto
	{
		[JsonPropertyName("material_type_id")]
		[Column("type_id", Visible = false)]
		public string MaterialTypeId { get; set; } = string.Empty;

		[JsonPropertyName("name")]
		[Column("Наименование", Visible = false)]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("display_name")]
		[Column("Тип комплектующих", Visible = true)]
		public string DisplayName { get; set; } = string.Empty;

		[JsonPropertyName("unit")]
		[Column("Ед.изм", Visible = false)]
		public string? Unit { get; set; }

		[JsonPropertyName("status_code")]
		[Column("Статус", Visible = false)]
		public string? StatusCode { get; set; }

		[JsonPropertyName("status_display")]
		[Column("Статус", Visible = true)]
		public string? StatusDisplay { get; set; }

		[JsonPropertyName("status_color")]
		public string? StatusColor { get; set; }

		[JsonPropertyName("expected_date")]
		[Column("Дата поставки", Visible = true)]
		public DateTime? ExpectedDate { get; set; }

		[JsonPropertyName("quantity")]
		[Column("Количество", Visible = false)]
		public double? Quantity { get; set; }

		[JsonPropertyName("comment")]
		[Column("Примечание", Visible = true)]
		public string? Comment { get; set; }

		public string QuantityDisplay => Quantity.HasValue ? $"{Quantity:F2} {Unit}" : "—";
		public string ExpectedDateDisplay => ExpectedDate?.ToString("dd.MM.yyyy") ?? "—";
	}
}