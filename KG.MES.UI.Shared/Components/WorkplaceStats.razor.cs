using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KG.MES.UI.Shared.Components;

public partial class WorkplaceStats
{
	[Inject] private ProductionApiService ApiService {get; set;} = null!;
	private List<WorkplaceDto> workplaces = [];
	private WorkplaceStatsDto? stats;
	private List<BlockedOrderDto> blocks = [];
	private List<WorkplaceHistoryDto> history = [];
	private Guid? selectedWorkplaceId;
	private DateTime dateFrom = DateTime.Now.AddDays(-7);
	private DateTime dateTo = DateTime.Now;
	private bool showDateFilter;

	protected override async Task OnInitializedAsync()
	{
		Console.WriteLine("WorkplaceStats OnInitializedAsync");
		workplaces = await ApiService.GetWorkplacesAsync("active");
	}

	private async Task SelectWorkplace(Guid id)
	{
		selectedWorkplaceId = id;
		stats = await ApiService.GetWorkplaceStatsAsync(id);
		blocks = await ApiService.GetWorkplaceBlocksAsync(id);
		history = await ApiService.GetWorkplaceHistoryAsync(id, dateFrom, dateTo, 1000);
		StateHasChanged();
	}

	private async Task ApplyFilter()
	{
		showDateFilter = false;

		if (selectedWorkplaceId == null) return;
		
		history = await ApiService.GetWorkplaceHistoryAsync(
			(Guid)selectedWorkplaceId, dateFrom, dateTo, 1000);
		StateHasChanged();
	}
}