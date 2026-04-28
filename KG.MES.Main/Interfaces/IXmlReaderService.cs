using XmlOrderReader.Web.Models;
using XmlOrderReader.Web.Models.Xml;

namespace XmlOrderReader.Web.Interfaces
{
    public interface IXmlReaderService
    {
        Task<XmlDocumentData> ReadXmlFromContent(string xmlContent);
        Task<XmlDocumentData> ReadXmlFromFile(string filePath);
    }
}