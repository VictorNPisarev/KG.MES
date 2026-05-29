namespace KG.MES.Shared.Models.Entities;

public class WorkplaceTransition
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public Guid FromWorkplaceId { get; set; }
	public Guid ToWorkplaceId { get; set; }
	public string TransitionType { get; set; } = "sequential";
	public DateTime CreatedAt { get; set; }

	// Navigation properties
	public Workplace? FromWorkplace { get; set; }
	public Workplace? ToWorkplace { get; set; }
}