using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class SupplyTypeDto
	{
		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
	}

	public static class SupplyTypeExtensions
	{
		public static string DisplayName(this SupplyTypeDto supplyType) => supplyType.Name switch
		{
			"lumber" => "Брус",
			"furniture" => "Фурнитура",
			"glass" => "Стекло",
			"paint" => "ЛКМ",
			"alumWaterShield" => "ППС, В/О",
			_ => supplyType.Name
		};
	}
}