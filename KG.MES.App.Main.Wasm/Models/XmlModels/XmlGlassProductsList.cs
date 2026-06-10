using System.Xml.Serialization;

namespace KG.MES.App.Main.Wasm.Models.Xml
{
	public partial class XmlGlassProductsList
	{
		[XmlElement("g_rect")]
		public List<XmlGlassProduct> XmlGlassRectangles { get; set; } = new();
		
		[XmlElement("g_spec")]
		public List<XmlGlassProduct> XmlGlassSpecials { get; set; } = new();		
	}

	public partial class XmlGlassProductsList
	{
		[XmlIgnore]
		public List<XmlGlassProduct> XmlGlassProducts => XmlGlassRectangles.Concat(XmlGlassSpecials).ToList();
	}

}