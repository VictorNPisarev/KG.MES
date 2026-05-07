using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class OrderSupplyDto
	{
		[JsonPropertyName("material_type_id")]
		public string MaterialTypeId { get; set; } = string.Empty;

		[JsonPropertyName("name")]
		[Column("Код", Visible = false)]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("display_name")]
		[Column("Материал", Order = 0)]
		public string DisplayName { get; set; } = string.Empty;

		[JsonPropertyName("quantity")]
		[Column("Количество", Order = 1)]
		public double? Quantity { get; set; }

		[JsonPropertyName("unit")]
		[Column("Ед.", Order = 2)]
		public string? Unit { get; set; }

		[JsonPropertyName("status_code")]
		[Column("Статус", Order = 3, IsBadge = true)]
		public string? StatusCode { get; set; }

		[JsonPropertyName("status_color")]
		public string? StatusColor { get; set; }

		[JsonPropertyName("expected_date")]
		[Column("Ожидаемая дата", Order = 4, DisplayFormat = "dd.MM.yyyy")]
		public DateTime? ExpectedDate { get; set; }

		[JsonPropertyName("comment")]
		[Column("Примечание", Order = 5)]
		public string? Comment { get; set; }

		public string QuantityDisplay => Quantity.HasValue ? $"{Quantity:F2} {Unit}" : "—";
		public string ExpectedDateDisplay => ExpectedDate?.ToString("dd.MM.yyyy") ?? "—";
	}

	public static class OrderSupplyDtoExtensions
	{
		public static string GetStatusCodeDisplayText(this OrderSupplyDto supply) => supply.StatusCode?.ToLower() switch
		{
			"not_available" => "Нет в наличии",
			"delayed" => "Задерживается",
			"ordered" => "Заказан",
			"in_stock" => "На складе",
			_ => supply.StatusCode ?? "—"
		};
	}

}