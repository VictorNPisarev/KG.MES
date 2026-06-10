using KG.MES.App.Main.Wasm.Models;
using KG.MES.App.Main.Wasm.Models.Xml;

namespace KG.MES.App.Main.Wasm.Interfaces
{
    public interface IXmlReaderService
    {
        Task<XmlDocumentData> ReadXmlFromContent(string xmlContent);
        Task<XmlDocumentData> ReadXmlFromFile(string filePath);
    }
}