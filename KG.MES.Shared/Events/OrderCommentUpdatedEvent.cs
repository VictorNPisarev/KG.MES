namespace KG.MES.Shared.Events;

public class OrderCommentUpdatedEvent
{
	public Guid OrderId { get; set; }
	public string? Source { get; set; } // 'order', 'production', 'supply'
}