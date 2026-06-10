using KG.MES.App.Main.Wasm.Models;
using KG.MES.App.Main.Wasm.Models.Xml;

namespace KG.MES.App.Main.Wasm.Interfaces
{
    public interface I1CExportService
    {
        Task<bool> ExportOrderAsync(XmlDocumentData order);
        Task<string> TestConnectionAsync();
    }
}