namespace KG.MES.Shared.Models.Entities;

public class SupplyType
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public string? Unit { get; set; }
	public int SortOrder { get; set; }
	public bool IsActive { get; set; } = true;
	public DateTime CreatedAt { get; set; }

	// Navigation properties
	public ICollection<SupplyItem>? SupplyItems { get; set; }
}