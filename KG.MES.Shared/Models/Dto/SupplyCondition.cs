using System.Text.Json.Serialization;
using KG.MES.Shared.Helpers;

namespace KG.MES.Shared.Models.Dto
{
	public class SupplyConditionDto
	{
		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;

		[JsonPropertyName("condition_code")]
		public string ConditionCode { get; set; } = string.Empty;
	}

	public static class SupplyConditionExtensions
	{
		public static string DisplayName(this SupplyConditionDto condition)
			=> BadgeHelper.GetDisplayValue(condition.ConditionCode, "supply_status");
	}
}