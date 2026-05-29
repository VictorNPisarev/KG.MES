namespace KG.MES.Shared.Models.Entities;

public class SupplyCondition
{
	public Guid Id { get; set; }
	public string ConditionCode { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	// Navigation properties
	public ICollection<SupplyItem>? SupplyItems { get; set; }
}