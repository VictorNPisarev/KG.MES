using System.ComponentModel;
using XmlOrderReader.Web.Common.Attributes;

namespace XmlOrderReader.Web.Models.Enums
{
	public enum FormType
	{
		[Description("Прямоугольная")]
		Rectangular,

		[Description("Арочная")]
		Arch,

		[Description("Косоугольная")]
		Sloped,

		[Description("Не определена")]
		Undefined
	}
}