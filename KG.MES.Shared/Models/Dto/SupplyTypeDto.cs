using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto;

public class SupplyTypeDto
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}

public static class SupplyTypeDtoExtensions
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

