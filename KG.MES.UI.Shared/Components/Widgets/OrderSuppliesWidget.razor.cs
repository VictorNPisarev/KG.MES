using Microsoft.AspNetCore.Components;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.UI.Shared.Interfaces;

namespace KG.MES.UI.Shared.Components.Widgets;

public partial class OrderSuppliesWidget : ComponentBase, ISavableWidget
{
	[Parameter] public Guid OrderId { get; set; }
	[Parameter] public bool CanEdit { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;
	[Inject] private SupplyService SupplyService { get; set; } = null!;

	private List<OrderSupply> supplies = new();
	private List<OrderSupply> originalSupplies = new();
	private List<OrderSupply> backup = new();
	private List<SupplyCondition> conditions = new();
	private List<SupplyType> types = new();

	private bool isLoading = true;
	private bool EditMode;
	private Dictionary<string, bool> showComments = new();
	private Dictionary<string, string> originalComments = new();
	private Dictionary<string, bool> openDropdowns = new();

	protected override async Task OnInitializedAsync()
	{
		conditions = await SupplyService.GetConditionsAsync();
		types = await SupplyService.GetTypesAsync();

		var supplyDtos = await ApiService.GetOrderSuppliesAsync(OrderId);
		originalSupplies = supplyDtos.Select(s => new OrderSupply(s, SupplyService)).ToList();
		supplies = supplyDtos.Select(s => new OrderSupply(s, SupplyService)).ToList();

		isLoading = false;
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
		else
		{
			foreach (var kvp in showComments.ToList())
			{
				var supply = supplies.FirstOrDefault(s => s.SupplyTypeId == kvp.Key);
				if (supply != null)
					await SaveComment(supply);
			}
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
		}
	}

	private async Task SaveComment(OrderSupply supply)
	{
		var original = originalSupplies.FirstOrDefault(o => o.SupplyTypeId == supply.SupplyTypeId);

		if (supply.Comment != original?.Comment)
		{
			var success = await ApiService.UpdateOrderSuppliesAsync(OrderId, new List<object>
		{
			new
			{
				supplyTypeId = supply.SupplyTypeId,
				supplyConditionId = supply.SupplyConditionId,
				comment = supply.Comment
			}
		});

			if (success)
			{
				// Обновляем оригинал
				if (original != null)
					original.Comment = supply.Comment;
			}
		}

		showComments.Remove(supply.SupplyTypeId);
	}
	
	public bool HasUnsavedChanges()
	{
		foreach (var s in supplies)
		{
			var original = originalSupplies.FirstOrDefault(o => o.SupplyTypeId == s.SupplyTypeId);
			if (original == null) continue;

			// Статус изменился
			if (s.SupplyConditionId != original.SupplyConditionId) return true;

			// Комментарий в объекте изменился (даже если поле ввода открыто и ещё не сохранено в объект)
			if (showComments.GetValueOrDefault(s.SupplyTypeId))
			{
				// Комментарий открыт — сравниваем с оригиналом
				if (s.Comment != original.Comment) return true;
			}
			else if (s.Comment != original.Comment) return true;
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
}