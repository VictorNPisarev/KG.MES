namespace KG.MES.Shared.Models.Entities;

public class Comment
{
	public Guid Id { get; set; }
	public Guid OrderId { get; set; }
	public Guid? UserId { get; set; }
	public string Content { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	// Navigation properties
	public Order? Order { get; set; }
	public User? User { get; set; }
}