using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto;

public class WorkplaceHistoryDto
{
	[JsonPropertyName("operation_time")]
	public DateTime OperationTime { get; set; }

	[JsonPropertyName("operation_type")]
	[Column("Операция", IsBadge = true)]
	public string OperationType { get; set; } = string.Empty;

	[JsonPropertyName("order_number")]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("user_name")]
	public string? UserName { get; set; }

	[JsonPropertyName("notes")]
	public string? Notes { get; set; }
}