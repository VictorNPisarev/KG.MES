using System.Threading.Tasks;
using KG.MES.Shared.Helpers;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;

public class OrderSupply
{
	private readonly SupplyService _supplyService;

	public string SupplyTypeId { get; private set; }
	public string? SupplyConditionId { get; private set; }
	public string? Comment { get; set; }

	// Справочные данные (заполняются извне)
	public SupplyType? SupplyType { get; private set; }
	public SupplyCondition? SupplyCondition { get; private set; }
	//[JsonIgnore] public string? ConditionBadge { get; private set; }

	public OrderSupply(OrderSupplyDto dto, SupplyService supplyService)
	{
		_supplyService = supplyService;
		SupplyTypeId = dto.SupplyTypeId;
		SupplyConditionId = dto.SupplyConditionId;
		Comment = dto.Comment;
		Enrich();
	}

	public void SetCondition(string conditionId)
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