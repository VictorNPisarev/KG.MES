using KG.MES.Shared.Interfaces;
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
	[Inject] private ISocketService SocketService { get; set; } = null!;

	private OrderTraceDto? orderTrace;
	private OrderTraceDto? backupTrace;
	private bool isLoading = true;
	private bool EditMode;
	private Dictionary<string, bool> openDropdowns = new();

	protected override async Task OnInitializedAsync()
	{
		SocketService.OnMessage += OnSocketMessage;
		orderTrace = await ApiService.GetOrderTraceAsync(OrderId);

		if (orderTrace?.ProductionOrderId != Guid.Empty)
		{
			await SocketService.SubscribeAsync("order", orderTrace?.ProductionOrderId.ToString());
		}

		isLoading = false;
	}

	private void OnSocketMessage(string channel, object data)
	{
		if (channel == "order")
		{
			InvokeAsync(async () =>
			{
				orderTrace = await ApiService.GetOrderTraceAsync(OrderId);
				StateHasChanged();
			});
		}
	}

	private void EnterEditMode()
	{
		backupTrace = new OrderTraceDto
		{
			OrderNumber = orderTrace?.OrderNumber ?? "",
			WorkplaceTraces = orderTrace?.WorkplaceTraces.Select(w => new WorkplaceTraceDto
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
		if (orderTrace == null || orderTrace.ProductionOrderId == Guid.Empty) return;

		foreach (var wp in orderTrace.WorkplaceTraces)
		{
			if (wp.WorkplaceId == Guid.Empty) continue;

			var original = backupTrace?.WorkplaceTraces.FirstOrDefault(b => b.WorkplaceId == wp.WorkplaceId);
			if (original == null || wp.Status != original.Status)
			{
				await ApiService.UpdateOrderTraceAsync(
					orderTrace.ProductionOrderId!,
					wp.WorkplaceId,
					wp.Status
				);
			}
		}

		orderTrace = await ApiService.GetOrderTraceAsync(OrderId);
		EditMode = false;
		openDropdowns.Clear();
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