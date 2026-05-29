namespace KG.MES.Shared.Models.Entities;

public class UserWorkplace
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public Guid UserId { get; set; }
	public Guid WorkplaceId { get; set; }
	public DateTime CreatedAt { get; set; }

	// Navigation properties
	public User? User { get; set; }
	public Workplace? Workplace { get; set; }
}