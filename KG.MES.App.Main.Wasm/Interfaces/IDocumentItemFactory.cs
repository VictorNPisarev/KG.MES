using KG.MES.App.Main.Wasm.Models;
using KG.MES.App.Main.Wasm.Models.Xml;

namespace KG.MES.App.Main.Wasm.Interfaces
{
	public interface IDocumentItemFactory
	{
		DocumentItem Create(XmlDocumentItem xmlData, DocumentData? documentData = null);
	}
}