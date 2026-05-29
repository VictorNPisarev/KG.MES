namespace KG.MES.Shared.Models.Entities;

public class SupplyItem
{
	public Guid Id { get; set; }
	public Guid OrderSupplyId { get; set; }
	public Guid SupplyTypeId { get; set; }
	public Guid? ConditionId { get; set; }
	public decimal? Quantity { get; set; }
	public DateTime? ExpectedDate { get; set; }
	public string? Comment { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public OrderSupply? OrderSupply { get; set; }
	public SupplyType? SupplyType { get; set; }
	public SupplyCondition? Condition { get; set; }
	public Guid? CommentId { get; set; }
	public Comment? CommentEntity { get; set; }
}