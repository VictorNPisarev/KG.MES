using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.Shared.Models.Enums;
using Microsoft.AspNetCore.Components;

namespace KG.MES.App.Main.Wasm.Pages;

public partial class CreateOrderPage
{
	private string OrderNumber { get; set; } = "";
	private int WindowCount { get; set; }
	private double WindowArea { get; set; }
	private int PlateCount { get; set; }
	private double PlateArea { get; set; }
	private string Machine { get; set; } = "";
	private DateTime StartDate { get; set; } = DateTime.Now;
	private int ApprovedDays { get; set; } = 14;
	private int UnapprovedDays { get; set; } = 21;
	private DateTime? So8Date { get; set; }
	private string Comment { get; set; } = "";
	private bool IsEconom { get; set; }
	private bool IsClaim { get; set; }
	private bool IsOnlyPaid { get; set; }
	private bool IsTwoSidePaint { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;

	private DateTime? ReadyDate
	{
		get
		{
			var days = ApprovedDays > 0 ? ApprovedDays : UnapprovedDays;
			return days > 0 ? StartDate.AddDays(days) : null;
		}
	}

	private bool isSaving;
	private string StatusMessage { get; set; } = "";
	private bool isError;

	private async Task SaveOrder()
	{
		if (string.IsNullOrWhiteSpace(OrderNumber))
		{
			StatusMessage = "Введите номер заказа";
			isError = true;
			return;
		}

		isSaving = true;
		StatusMessage = "";

		try
		{
			var dto = new ProductionOrderExportDto
			{
				OrderNumber = OrderNumber,
				WindowCount = WindowCount,
				WindowArea = WindowArea,
				PlateCount = PlateCount,
				PlateArea = PlateArea,
				Comment = Comment,
				IsEconom = IsEconom,
				IsClaim = IsClaim,
				IsOnlyPaid = IsOnlyPaid,
				IsTwoSidePaint = IsTwoSidePaint,
				StartDate = StartDate,
				ProductionDays = ApprovedDays > 0 ? ApprovedDays : UnapprovedDays,
				ReadyDate = ReadyDate
			};

			var success = await ApiService.ExportToProductionAsync(dto);

			if (success)
			{
				StatusMessage = $"Заказ №{OrderNumber} создан";
				isError = false;
				// Очистка формы
				OrderNumber = "";
				WindowCount = 0;
				WindowArea = 0;
				PlateCount = 0;
				PlateArea = 0;
				Comment = "";
				IsEconom = false;
				IsClaim = false;
				IsOnlyPaid = false;
			}
			else
			{
				StatusMessage = "Ошибка при создании заказа";
				isError = true;
			}
		}
		catch (Exception ex)
		{
			StatusMessage = $"Ошибка: {ex.Message}";
			isError = true;
		}
		finally
		{
			isSaving = false;
		}
	}
}