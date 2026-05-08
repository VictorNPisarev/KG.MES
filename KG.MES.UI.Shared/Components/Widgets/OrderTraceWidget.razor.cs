using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.UI.Shared.Interfaces;
using Microsoft.AspNetCore.Components;

namespace KG.MES.UI.Shared.Components.Widgets;

public partial class OrderTraceWidget : ComponentBase, ISavableWidget
{
	[Parameter] public Guid OrderId { get; set; }

	[Inject] private ProductionApiService ApiService {get; set;} = null!;


	private OrderTrace? orderTrace;
	private bool isLoading = true;

	protected override async Task OnInitializedAsync()
	{
		orderTrace = await ApiService.GetOrderTraceAsync(OrderId);
		isLoading = false;
	}

	public bool HasUnsavedChanges() => false;
	public Task SaveAllAsync() => Task.CompletedTask;

}