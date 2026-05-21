using Microsoft.AspNetCore.Components;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.UI.Shared.Interfaces;
using KG.MES.Shared.Interfaces;
using KG.MES.Shared.Events;
using Microsoft.Extensions.Logging;

namespace KG.MES.UI.Shared.Components.Widgets;

public partial class OrderSuppliesWidget : ComponentBase, ISavableWidget
{
	[Parameter] public Guid OrderId { get; set; }
	[Parameter] public bool CanEdit { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;
	[Inject] private SupplyService SupplyService { get; set; } = null!;
	[Inject] private IEventAggregator EventAggregator { get; set; } = null!;

	private List<OrderSupply> supplies = [];
	private List<OrderSupply> originalSupplies = [];
	private List<OrderSupply> backup = [];
	private List<SupplyCondition> conditions = [];
	private List<SupplyType> types = [];

	private bool isLoading = true;
	private bool EditMode;
	private Dictionary<string, bool> showComments = [];
	private Dictionary<string, string> originalComments = [];
	private Dictionary<string, bool> openDropdowns = [];

	protected override async Task OnInitializedAsync()
	{
		//Подписываюсь на изменение комментария
		EventAggregator.Subscribe<OrderCommentUpdatedEvent>(OnOrderCommentUpdated);

		conditions = await SupplyService.GetConditionsAsync();
		types = await SupplyService.GetTypesAsync();

		await LoadSupplies();

		isLoading = false;
	}

	private async Task LoadSupplies()
	{
		var supplyDtos = await ApiService.GetOrderSuppliesAsync(OrderId);
		originalSupplies = supplyDtos.Select(s => new OrderSupply(s, SupplyService)).ToList();
		supplies = supplyDtos.Select(s => new OrderSupply(s, SupplyService)).ToList();

		StateHasChanged();
	}

	private void EnterEditMode()
	{
		backup = supplies.Select(s => new OrderSupply(s.ToDto(), SupplyService)).ToList();
		EditMode = true;
	}

	private void CancelEdit()
	{
		supplies = backup.Select(b => new OrderSupply(b.ToDto(), SupplyService)).ToList();
		backup.Clear();
		EditMode = false;
		showComments.Clear();
		originalComments.Clear();
		openDropdowns.Clear();
	}

	private void SelectCondition(OrderSupply supply, string conditionId)
	{
		supply.SetCondition(conditionId);
	}

	private void ToggleDropdown(string key)
	{
		var wasOpen = openDropdowns.GetValueOrDefault(key);
		openDropdowns.Clear();
		if (!wasOpen)
			openDropdowns[key] = true;
	}

	private void CloseDropdown(string key)
	{
		openDropdowns.Remove(key);
	}

	private void ToggleComment(OrderSupply supply)
	{
		var key = supply.SupplyTypeId;
		if (showComments.GetValueOrDefault(key))
		{
			showComments.Remove(key);
		}
		else
		{
			originalComments[key] = supply.Comment ?? "";
			showComments[key] = true;
		}
	}

	public async Task SaveAllAsync()
	{
		if (EditMode)
		{
			await SaveChanges();
		}
	}

	private async Task SaveChanges()
	{
		var updates = new List<object>();

		foreach (var s in supplies)
		{
			var original = backup.FirstOrDefault(b => b.SupplyTypeId == s.SupplyTypeId);
			if (original != null)
			{
				if (s.SupplyConditionId != original.SupplyConditionId || s.Comment != original.Comment)
				{
					updates.Add(new
					{
						supplyTypeId = s.SupplyTypeId,
						supplyConditionId = s.SupplyConditionId,
						comment = s.Comment
					});
				}
			}

			if (showComments.GetValueOrDefault(s.SupplyTypeId))
			{
				var origComment = originalComments.GetValueOrDefault(s.SupplyTypeId) ?? "";
				if (s.Comment != origComment)
				{
					updates.Add(new
					{
						supplyTypeId = s.SupplyTypeId,
						supplyConditionId = s.SupplyConditionId,
						comment = s.Comment
					});
				}
			}
		}

		updates = updates
			.GroupBy(u => ((dynamic)u).supplyTypeId)
			.Select(g => g.First())
			.ToList();

		if (!updates.Any())
		{
			ClearState();
			return;
		}

		var success = await ApiService.UpdateOrderSuppliesAsync(OrderId, updates);

		if (success)
		{
			ClearState();
			var supplyDtos = await ApiService.GetOrderSuppliesAsync(OrderId);
			originalSupplies = supplyDtos.Select(s => new OrderSupply(s, SupplyService)).ToList(); 
			supplies = supplyDtos.Select(s => new OrderSupply(s, SupplyService)).ToList();

			// Публикую событие
			EventAggregator.Publish(new OrderCommentUpdatedEvent
			{
				OrderId = OrderId,
				Source = "supply"
			});
		}
	}

	private async Task SaveComment(OrderSupply supply)
	{
		var original = originalSupplies.FirstOrDefault(o => o.SupplyTypeId == supply.SupplyTypeId);
		var success = false;

		Console.WriteLine($"original?.Comment: {original?.Comment}");

		if (original == null)
		{
			Console.WriteLine("original == null");
			success = await ApiService.UpdateOrderSuppliesAsync(OrderId,
			[
				new
				{
					supplyTypeId = supply.SupplyTypeId,
					supplyConditionId = supply.SupplyConditionId,
					comment = supply.Comment
				}
			]);
		}
		else if (supply.Comment != original?.Comment && supply.CommentId != null)
		{
			Console.WriteLine("supply.Comment != original?.Comment && supply.CommentId != null");
			success = await ApiService.UpdateCommentAsync(OrderId,
			new OrderCommentDto
			{
				Id = supply.CommentId ?? new Guid(),
				Content = supply.Comment
			}
			);
		}

		Console.WriteLine($"success: {success}");

		if (success)
		{
			// Обновляю оригинал
			original?.Comment = supply.Comment;

			// Публикую событие
			EventAggregator.Publish(new OrderCommentUpdatedEvent
			{
				OrderId = OrderId,
				Source = "supply"
			});
		}

		showComments.Remove(supply.SupplyTypeId);
	}

	public bool HasUnsavedChanges()
	{
		if (!EditMode) return false;

		foreach (var s in supplies)
		{
			var original = backup.FirstOrDefault(b => b.SupplyTypeId == s.SupplyTypeId);
			if (original == null) continue;
			if (s.SupplyConditionId != original.SupplyConditionId || s.Comment != original.Comment) return true;
		}

		return false;
	}

	private void ClearState()
	{
		EditMode = false;
		showComments.Clear();
		originalComments.Clear();
		openDropdowns.Clear();
		backup.Clear();
	}

	private async void OnOrderCommentUpdated(OrderCommentUpdatedEvent eventData)
	{
		if (eventData.OrderId == OrderId)
		{
			await LoadSupplies();
			await InvokeAsync(StateHasChanged);
		}
	}
	public void Dispose()
	{
		EventAggregator.Unsubscribe<OrderCommentUpdatedEvent>(OnOrderCommentUpdated);
	}

}