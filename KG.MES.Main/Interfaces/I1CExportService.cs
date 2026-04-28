using XmlOrderReader.Web.Models;
using XmlOrderReader.Web.Models.Xml;

namespace XmlOrderReader.Web.Interfaces
{
    public interface I1CExportService
    {
        Task<bool> ExportOrderAsync(XmlDocumentData order);
        Task<string> TestConnectionAsync();
    }
}