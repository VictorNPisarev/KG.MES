using KG.MES.Shared.Events;
using KG.MES.Shared.Interfaces;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.UI.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KG.MES.UI.Shared.Components.Widgets;

public partial class OrderCommentsWidget : ComponentBase, ISavableWidget, IDisposable
{
	[Parameter] public Guid OrderId { get; set; }

	[Inject] ProductionApiService ApiService { get; set; } = null!;
	[Inject] IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] private IEventAggregator EventAggregator { get; set; } = null!;

	private List<OrderCommentDto> comments = new();
	private List<OrderCommentDto> originalComments = new();
	private bool isLoading = true;

	protected override async Task OnInitializedAsync()
	{
		EventAggregator.Subscribe<OrderCommentUpdatedEvent>(OnOrderCommentUpdated);
		await LoadComments();
	}

	private async Task LoadComments()
	{
		isLoading = true;
		StateHasChanged();
		comments = await ApiService.GetOrderCommentsAsync(OrderId);
		// Глубокая копия для отслеживания изменений
		originalComments = comments.Select(c => new OrderCommentDto
		{
			Id = c.Id,
			Content = c.Content,
			CreatedAt = c.CreatedAt,
			UpdatedAt = c.UpdatedAt,
			UserName = c.UserName
		}).ToList();
		isLoading = false;
		StateHasChanged();
	}

	private void AddNewComment()
	{
		var newComment = new OrderCommentDto
		{
			Id = Guid.NewGuid(), // временный ID
			IsNew = true,
			IsEditing = true,
			CreatedAt = DateTime.UtcNow
		};
		comments.Add(newComment);
	}

	private void EditComment(OrderCommentDto comment)
	{
		comment.IsEditing = true;
	}

	private async Task SaveComment(OrderCommentDto comment)
	{
		Console.WriteLine($"SaveComment OrderId: {OrderId}");
		var success = await ApiService.SaveCommentAsync(OrderId, comment);//SaveSupplyCommentAsync(OrderId, comment);
		if (success)
		{
			// Перезагружаем комментарии, чтобы получить реальный ID и данные
			await LoadComments();

			// Публикую событие
			EventAggregator.Publish(new OrderCommentUpdatedEvent
			{
				OrderId = OrderId,
				Source = "supply"
			});

		}
	}

	private void CancelEditComment(OrderCommentDto comment)
	{
		if (comment.IsNew)
		{
			comments.Remove(comment);
		}
		else
		{
			// Восстанавливаем из оригинала
			var original = originalComments.FirstOrDefault(o => o.Id == comment.Id);
			if (original != null)
			{
				comment.Content = original.Content;
			}
			comment.IsEditing = false;
		}
	}

	private async Task DeleteComment(OrderCommentDto comment)
	{
		if (comment.IsNew)
		{
			comments.Remove(comment);
			return;
		}

		var success = await ApiService.DeleteCommentAsync(OrderId, comment.Id);
		if (success)
		{
			comments.Remove(comment);
			originalComments.RemoveAll(o => o.Id == comment.Id);
		}
	}

	public bool HasUnsavedChanges()
	{
		// Есть ли новые комментарии или изменённые
		if (comments.Any(c => c.IsNew)) return true;

		foreach (var c in comments)
		{
			var original = originalComments.FirstOrDefault(o => o.Id == c.Id);
			if (original == null) continue;
			if (c.Content != original.Content) return true;
		}

		return false;
	}

	public async Task SaveAllAsync()
	{
		// Сохраняем все изменённые и новые
		foreach (var c in comments.ToList())
		{
			if (c.IsNew || c.Content != originalComments.FirstOrDefault(o => o.Id == c.Id)?.Content)
			{
				await SaveComment(c);
			}
		}
	}

	private async void OnOrderCommentUpdated(OrderCommentUpdatedEvent eventData)
	{
		if (eventData.OrderId == OrderId)
		{
			await LoadComments(); // перезагружаем комментарии
			await InvokeAsync(StateHasChanged);
		}
	}

	public void Dispose()
	{
		EventAggregator.Unsubscribe<OrderCommentUpdatedEvent>(OnOrderCommentUpdated);
	}
}