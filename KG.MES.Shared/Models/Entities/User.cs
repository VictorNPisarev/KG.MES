namespace KG.MES.Shared.Models.Entities;

public class User
{
	public Guid Id { get; set; }
	public string? LegacyId { get; set; }
	public string Email { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public Guid? RoleId { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public Role? Role { get; set; }
	public ICollection<UserWorkplace>? UserWorkplaces { get; set; }
	public ICollection<OrderBlock>? OrderBlocks { get; set; }
	public ICollection<OperationLog>? OperationLogs { get; set; }
	public ICollection<Comment>? Comments { get; set; }
}