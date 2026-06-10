using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto;

public class WorkplaceBlockDto
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("production_order_id")]
	public Guid ProductionOrderId { get; set; }

	[JsonPropertyName("order_number")]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("reason")]
	public string? Reason { get; set; }

	[JsonPropertyName("blocked_at")]
	public DateTime BlockedAt { get; set; }

	[JsonPropertyName("user_name")]
	public string? UserName { get; set; }
}