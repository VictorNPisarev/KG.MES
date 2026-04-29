using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class WorkplaceDto
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("type")]
		public string? Type { get; set; }

		[JsonPropertyName("is_active")]
		public bool IsActive { get; set; }
	}
}