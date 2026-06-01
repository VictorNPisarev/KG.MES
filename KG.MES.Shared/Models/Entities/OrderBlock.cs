namespace KG.MES.Shared.Models.Entities;

public class OrderBlock
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public Guid ProductionOrderId { get; set; }
	public Guid WorkplaceId { get; set; }
	public Guid? UserId { get; set; }
	public string? Reason { get; set; }
	public DateTime BlockedAt { get; set; }
	public DateTime? ResolvedAt { get; set; }
	public Guid? ResolvedBy { get; set; }
	public string? Source { get; set; }

	// Navigation properties
	public ProductionOrder? ProductionOrder { get; set; }
	public Workplace? Workplace { get; set; }
	public User? User { get; set; }
	//public User? ResolvedByUser { get; set; }
}