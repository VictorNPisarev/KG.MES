using XmlOrderReader.Web.Models;
using XmlOrderReader.Web.Models.Xml;

namespace XmlOrderReader.Web.Interfaces
{
	public interface IDocumentItemFactory
	{
		DocumentItem Create(XmlDocumentItem xmlData, DocumentData? documentData = null);
	}
}