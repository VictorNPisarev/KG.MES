using System.Xml.Serialization;
using KG.MES.App.Main.Wasm.Interfaces;
using KG.MES.App.Main.Wasm.Models.Enums;

namespace KG.MES.App.Main.Wasm.Models.Xml
{
	public partial class XmlAccessory
	{
		[XmlElement("article_number")]
		public string? AccessoryArticleNumber { get; set; }
		
		[XmlElement("item_text")]
		public string? AccessoryDescription { get; set; }
		
		[XmlElement("item_prime_cost")]
		public decimal Price { get; set; }
		
		[XmlElement("item_quantity")]
		public decimal Quantity { get; set; }
		
		[XmlElement("piece")]
		public int Piece { get; set; }
		
		[XmlElement("unit")]
		public string? XmlUnit { get; set; }
	}

	
	/// <summary>
	/// Реализация интерфейса + поля из вложенных блоков
	/// </summary>
	public partial class XmlAccessory : IXmlDataMaterial
	{
		[XmlIgnore]
		public XmlBlockType XmlBlockType => XmlBlockType.XmlAccessory;

		[XmlIgnore]
		public string? ArticleNumber => AccessoryArticleNumber;

		[XmlIgnore]
		public string? Name => AccessoryDescription;

		public int PieceCount => Piece;

		public decimal UnitPrice => Price;

		public string? Unit => XmlUnit;

		public void Validate() {}
	}

}