namespace KG.MES.Shared.Models.Entities;

public class OrderSupply
{
	public Guid Id { get; set; }
	public Guid OrderId { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public Order? Order { get; set; }
	public ICollection<Guid>? CommentIds { get; set; }
	public ICollection<SupplyItem>? SupplyItems { get; set; }
}