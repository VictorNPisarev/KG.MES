using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	/// <summary>
	/// Обертка в JSON
	/// </summary>
	public class OrderTraceResponse
	{
		[JsonPropertyName("orders")]
		public List<OrderTrace> OrderTraces { get; set; } = new();
	}


	public class OrderTrace
	{
		[JsonPropertyName("orderId")]
		public Guid OrderId { get; set; }

		[JsonPropertyName("productionOrderId")]
		public string? ProductionOrderId { get; set; }

		[JsonPropertyName("orderNumber")]
		public string OrderNumber { get; set; } = string.Empty;

		[JsonPropertyName("readyDate")]
		public DateTime? ReadyDate { get; set; }

		[JsonPropertyName("workplaces")]
		public List<WorkplaceTrace> WorkplaceTraces { get; set; } = new();
	}
}