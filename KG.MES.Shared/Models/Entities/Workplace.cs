namespace KG.MES.Shared.Models.Entities;

public class Workplace
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public string Name { get; set; } = string.Empty;
	public Guid? PreviousWorkplaceId { get; set; }
	public bool IsWorkplace { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public int Level { get; set; }

	// Navigation properties
	public Workplace? PreviousWorkplace { get; set; }
	public ICollection<WorkplaceTransition>? FromTransitions { get; set; }
	public ICollection<WorkplaceTransition>? ToTransitions { get; set; }
	public ICollection<OrderFootprint>? OrderFootprints { get; set; }
	public ICollection<OrderBlock>? OrderBlocks { get; set; }
	public ICollection<OperationLog>? OperationLogs { get; set; }
	public ICollection<ProductionOrder>? ProductionOrders { get; set; }
	public ICollection<UserWorkplace>? UserWorkplaces { get; set; }
}