namespace KG.MES.Shared.Models.Entities;

public class OperationLog
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public Guid ProductionOrderId { get; set; }
	public Guid WorkplaceId { get; set; }
	public Guid? UserId { get; set; }
	public string OperationType { get; set; } = string.Empty;
	public DateTime OperationTime { get; set; }
	public string? Notes { get; set; }
	public string? Source { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation properties
	public ProductionOrder? ProductionOrder { get; set; }
	public Workplace? Workplace { get; set; }
	public User? User { get; set; }
}