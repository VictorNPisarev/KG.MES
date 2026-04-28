// Common/Attributes/ColorAttribute.cs
namespace KG.MES.Main.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ColorAttribute : Attribute
	{
		public string ColorName { get; }
		public ColorAttribute(string colorName) => ColorName = colorName;
	}
}