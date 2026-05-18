using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class SupplyOrderDto
	{
		[JsonPropertyName("id")]
		[Column("ID", Visible = false)]
		public Guid Id { get; set; }

		[JsonPropertyName("order_number")]
		[Column("№ заказа", Order = 0)]
		public string OrderNumber { get; set; } = string.Empty;

		[JsonPropertyName("ready_date")]
		[Column("Готовность", Order = 1, DisplayFormat = "dd.MM.yyyy")]
		public DateTime? ReadyDate { get; set; }

		[JsonPropertyName("lumber")]
		[Column("Пиломатериалы", Order = 2, IsBadge = true)]
		public string? Lumber { get; set; }

		[JsonPropertyName("paint")]
		[Column("ЛКМ", Order = 3, IsBadge = true)]
		public string? Paint { get; set; }

		[JsonPropertyName("glass")]
		[Column("Стекло", Order = 4, IsBadge = true)]
		public string? Glass { get; set; }

		[JsonPropertyName("furniture")]
		[Column("Фурнитура", Order = 5, IsBadge = true)]
		public string? Furniture { get; set; }

		[JsonPropertyName("alumwatershield")]
		[Column("ППС, В/О", Order = 6, IsBadge = true)]
		public string? AlumWaterShield { get; set; }
	}
}