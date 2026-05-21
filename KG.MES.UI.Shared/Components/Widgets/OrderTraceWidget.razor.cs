using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.UI.Shared.Interfaces;
using Microsoft.AspNetCore.Components;

namespace KG.MES.UI.Shared.Components.Widgets;

public partial class OrderTraceWidget : ComponentBase, ISavableWidget
{
	[Parameter] public Guid OrderId { get; set; }
	[Parameter] public bool CanEdit { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;

	private OrderTrace? orderTrace;
	private OrderTrace? backupTrace;
	private bool isLoading = true;
	private bool EditMode;
	private Dictionary<string, bool> openDropdowns = new();

	protected override async Task OnInitializedAsync()
	{
		orderTrace = await ApiService.GetOrderTraceAsync(OrderId);
		isLoading = false;
	}

	private void EnterEditMode()
	{
		backupTrace = new OrderTrace
		{
			OrderNumber = orderTrace?.OrderNumber ?? "",
			WorkplaceTraces = orderTrace?.WorkplaceTraces.Select(w => new WorkplaceTrace
			{
				WorkplaceId = w.WorkplaceId,
				WorkplaceName = w.WorkplaceName,
				Status = w.Status
			}).ToList() ?? new()
		};
		EditMode = true;
	}

	private void CancelEdit()
	{
		if (backupTrace != null)
		{
			orderTrace = backupTrace;
		}
		EditMode = false;
		openDropdowns.Clear();
	}

	private async Task SaveChanges()
	{
		if (orderTrace == null) return;

		var updates = orderTrace.WorkplaceTraces.Select(w => new
		{
			workplaceId = w.WorkplaceId,
			status = w.Status
		}).ToList();

		var success = await ApiService.UpdateOrderTraceAsync(OrderId, updates);

		if (success)
		{
			orderTrace = await ApiService.GetOrderTraceAsync(OrderId);
			EditMode = false;
			openDropdowns.Clear();
		}
	}

	private void ToggleDropdown(string key)
	{
		var wasOpen = openDropdowns.GetValueOrDefault(key);
		openDropdowns.Clear();
		if (!wasOpen) openDropdowns[key] = true;
	}

	private void CloseDropdown(string key)
	{
		openDropdowns.Remove(key);
	}

	public bool HasUnsavedChanges() => EditMode;
	public Task SaveAllAsync() => SaveChanges();
}