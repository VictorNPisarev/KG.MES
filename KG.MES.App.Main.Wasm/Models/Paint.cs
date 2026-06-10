using System.Xml.Serialization;
using KG.MES.App.Main.Wasm.Models.Xml;

namespace KG.MES.App.Main.Wasm.Models
{ 
	public class Paint
	{
		/// <summary>
		/// Наименование цвета
		/// </summary>
		public string? PaintNumber { get; set; }
		
		/// <summary>
		/// Количество, м2
		/// </summary>
		public decimal QuantitySqm { get; set; }
		
		/// <summary>
		/// Количество, м
		/// </summary>
		public decimal QuantityM { get; set; }
		
		/// <summary>
		/// Цена
		/// </summary>
		public decimal Price { get; set; }

		public Paint() {}

		public Paint(XmlPaint xmlPaint)
		{
			PaintNumber = xmlPaint.PaintNumber;
			QuantitySqm = xmlPaint.QuantitySqm;
			QuantityM = xmlPaint.QuantityM;
			Price = xmlPaint.Price;
		}
	}
}