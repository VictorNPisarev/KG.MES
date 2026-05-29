namespace KG.MES.Shared.Models.Entities;

public class ProductionOrder
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public Guid OrderId { get; set; }
	public Guid? CurrentWorkplaceId { get; set; }
	public string? Comment { get; set; }
	public string? Lumber { get; set; }
	public string? GlazingBead { get; set; }
	public bool IsTwoSidePaint { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public Order? Order { get; set; }
	public Workplace? CurrentWorkplace { get; set; }
	public ICollection<Guid>? CommentIds { get; set; }
	public ICollection<OrderFootprint>? OrderFootprints { get; set; }
	public ICollection<OperationLog>? OperationLogs { get; set; }
	public ICollection<OrderBlock>? OrderBlocks { get; set; }
}