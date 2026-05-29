namespace KG.MES.Shared.Models.Entities;

public class Order
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public string OrderNumber { get; set; } = string.Empty;
	public DateTime? ReadyDate { get; set; }
	public int WindowCount { get; set; }
	public decimal WindowArea { get; set; }
	public int PlateCount { get; set; }
	public decimal PlateArea { get; set; }
	public bool IsEconom { get; set; }
	public bool IsClaim { get; set; }
	public bool IsOnlyPaid { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public ICollection<Guid>? CommentIds { get; set; }
	public ProductionOrder? ProductionOrder { get; set; }
	public OrderSupply? OrderSupply { get; set; }
}