// Models/IndicatorCategory.cs
using System.Text.Json.Serialization;

namespace XmlOrderReader.Web.Models
{
	public class IndicatorCategory
	{
		[JsonPropertyName("categoryKey")]
		public string CategoryKey { get; set; } = string.Empty;
		
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		
		[JsonPropertyName("indicators")]
		public List<string> Indicators { get; set; } = new();
	}

	public class CategoriesRoot
	{
		[JsonPropertyName("categories")]
		public List<IndicatorCategory> Categories { get; set; } = new();
	}
}
