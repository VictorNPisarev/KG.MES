using System.Threading.Tasks;
using KG.MES.Shared.Helpers;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;

namespace KG.MES.Shared.ViewModels;

public class OrderSupplyViewModel
{
	private readonly SupplyService _supplyService;

	public Guid SupplyTypeId { get; private set; }
	public Guid? SupplyConditionId { get; private set; }
	public string? Comment { get; set; }

	public Guid? CommentId { get; set; }

	// Справочные данные (заполняются извне)
	public SupplyTypeDto? SupplyType { get; private set; }
	public SupplyConditionDto? SupplyCondition { get; private set; }
	//[JsonIgnore] public string? ConditionBadge { get; private set; }

	public OrderSupplyViewModel(OrderSupplyDto dto, SupplyService supplyService)
	{
		_supplyService = supplyService;
		SupplyTypeId = dto.SupplyTypeId;
		SupplyConditionId = dto.SupplyConditionId;
		Comment = dto.Comment;
		CommentId = dto.CommentId;
		Enrich();
	}

	public void SetCondition(Guid conditionId)
	{
		SupplyConditionId = conditionId;
		Enrich();
	}

	private async Task Enrich()
	{
		var types = await _supplyService.GetTypesAsync();
		var conditions = await _supplyService.GetConditionsAsync();

		SupplyType = types.FirstOrDefault(t => t.Id == SupplyTypeId);
		SupplyCondition = conditions.FirstOrDefault(c => c.Id == SupplyConditionId);
	}

	public OrderSupplyDto ToDto() => new()
	{
		SupplyTypeId = SupplyTypeId,
		SupplyConditionId = SupplyConditionId,
		Comment = Comment
	};
}