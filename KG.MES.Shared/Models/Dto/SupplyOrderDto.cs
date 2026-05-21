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
		[Column("Пиломатериалы", Order = 2, IsBadge = true, CommentField = "LumberComment")]
		public string? Lumber { get; set; }

		[JsonPropertyName("lumber_comment")]
		public string? LumberComment { get; set; }

		[JsonPropertyName("paint")]
		[Column("ЛКМ", Order = 3, IsBadge = true, CommentField = "PaintComment")]
		public string? Paint { get; set; }

		[JsonPropertyName("paint_comment")]
		public string? PaintComment { get; set; }

		[JsonPropertyName("glass")]
		[Column("Стекло", Order = 4, IsBadge = true, CommentField = "GlassComment")]
		public string? Glass { get; set; }

		[JsonPropertyName("glass_comment")]
		public string? GlassComment { get; set; }

		[JsonPropertyName("furniture")]
		[Column("Фурнитура", Order = 5, IsBadge = true, CommentField = "FurnitureComment")]
		public string? Furniture { get; set; }

		[JsonPropertyName("furniture_comment")]
		public string? FurnitureComment { get; set; }

		[JsonPropertyName("alumwatershield")]
		[Column("ППС, В/О", Order = 6, IsBadge = true, CommentField = "AlumWaterShieldComment")]
		public string? AlumWaterShield { get; set; }

		[JsonPropertyName("alumWaterShield_comment")]
		public string? AlumWaterShieldComment { get; set; }
	}
}