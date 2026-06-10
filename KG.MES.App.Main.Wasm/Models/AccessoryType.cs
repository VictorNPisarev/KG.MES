using System.ComponentModel;

namespace KG.MES.App.Main.Wasm.Models
{
	public enum AccessoryType
	{
		[Description("В составе конструкции")]
		ArtTech,
		[Description("Дополнение к позиции")]
		AccessoryItem
	}
}