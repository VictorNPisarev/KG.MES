// Models/MaterialConfig.cs
using System.Text.Json.Serialization;

namespace XmlOrderReader.Web.Models
{
	public class MaterialTypeConfig
	{
		[JsonPropertyName("types")]
		public Dictionary<string, MaterialType> Types { get; set; } = new();
		
		[JsonPropertyName("rules")]
		public List<MaterialTypeRule> Rules { get; set; } = new();
	}
}