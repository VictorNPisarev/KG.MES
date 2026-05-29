namespace KG.MES.Shared.Models.Entities;

public class Role
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int Level { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public ICollection<User>? Users { get; set; }
}