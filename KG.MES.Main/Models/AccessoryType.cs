using System.ComponentModel;

namespace XmlOrderReader.Web.Models
{
	public enum AccessoryType
	{
		[Description("В составе конструкции")]
		ArtTech,
		[Description("Дополнение к позиции")]
		AccessoryItem
	}
}