using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	/// <summary>
	/// Обертка в JSON
	/// </summary>
	public class OrderTraceResponse
	{
		[JsonPropertyName("orders")]
		public List<OrderTraceDto> OrderTraces { get; set; } = new();
	}


	public class OrderTraceDto
	{
		[JsonPropertyName("orderId")]
		public Guid OrderId { get; set; }

		[JsonPropertyName("productionOrderId")]
		public Guid ProductionOrderId { get; set; }

		[JsonPropertyName("orderNumber")]
		public string OrderNumber { get; set; } = string.Empty;

		[JsonPropertyName("readyDate")]
		public DateTime? ReadyDate { get; set; }

		[JsonPropertyName("workplaces")]
		public List<WorkplaceTraceDto> WorkplaceTraces { get; set; } = new();
	}
}