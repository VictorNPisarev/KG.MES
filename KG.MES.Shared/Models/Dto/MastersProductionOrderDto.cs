using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class MastersProductionOrderDto
	{
		[JsonPropertyName("id")]
		[Column("ID", Visible = false)]
		public Guid Id { get; set; }

		[JsonPropertyName("order_number")]
		[Column("№ заказа", Order = 1)]
		public string OrderNumber { get; set; } = string.Empty;

		[JsonPropertyName("ready_date")]
		[Column("Готовность", Order = 2, DisplayFormat = "dd.MM.yyyy")]
		public DateTime? ReadyDate { get; set; }

		[JsonPropertyName("current_status")]
		[Column("Статус", Order = 3, IsBadge = true)]
		public string? Status { get; set; }

		[JsonPropertyName("window_count")]
		[Column("Окна, шт", Order = 4)]
		public int WindowCount { get; set; }

		[JsonPropertyName("window_area")]
		[Column("Окна, м2", Order = 5, DisplayFormat = "F2")]
		public double? WindowArea { get; set; }

		[JsonPropertyName("plate_count")]
		[Column("Щитовые, шт", Order = 6)]
		public int PlateCount { get; set; }

		[JsonPropertyName("plate_area")]
		[Column("Щитовые, м2", Order = 7, DisplayFormat = "F2")]
		public double? PlateArea { get; set; }

		[JsonPropertyName("created_at")]
		[Column("Дата запуска", Visible = false)]
		public DateTime StartDate { get; set; }

		[JsonPropertyName("is_econom")]
		[Column("Эконом", Visible = false, IsBadge = true)]
		public bool IsEconom { get; set; }

		[JsonPropertyName("is_claim")]
		[Column("Рекламация", Visible = false, IsBadge = true)]
		public bool IsClaim { get; set; }

		[JsonPropertyName("is_only_paid")]
		[Column("Оплачен, не запущен", Visible = false, IsBadge = true)]
		public bool IsOnlyPaid { get; set; }

		[JsonPropertyName("production_order_id")]
		[Column("Контрагент", Visible = false)]
		public string? ProductionOrderId { get; set; }

		[JsonPropertyName("current_workplace_id")]
		[Column("Контрагент", Visible = false)]
		public string? CurrentWorkplaceId { get; set; }

		[JsonPropertyName("customer_name")]
		[Column("Контрагент", Visible = false)]
		public string CustomerName { get; set; } = string.Empty;

		[JsonPropertyName("current_workplace_name")]
		[Column("Участок", Order = 8)]
		public string? CurrentWorkplaceName { get; set; }

		[JsonPropertyName("master_notes")]
		[Column("Заметки мастера", Order = 9)]
		public string? MasterNotes { get; set; }
	}

}