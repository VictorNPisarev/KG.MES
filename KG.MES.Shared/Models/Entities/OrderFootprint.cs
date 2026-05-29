namespace KG.MES.Shared.Models.Entities;

public class OrderFootprint
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public Guid ProductionOrderId { get; set; }
	public Guid WorkplaceId { get; set; }
	public string Status { get; set; } = "planned";
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public ProductionOrder? ProductionOrder { get; set; }
	public Workplace? Workplace { get; set; }
}