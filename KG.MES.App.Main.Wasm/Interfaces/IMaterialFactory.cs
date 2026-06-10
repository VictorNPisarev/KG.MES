using KG.MES.App.Main.Wasm.Models;

namespace KG.MES.App.Main.Wasm.Interfaces
{
	public interface IMaterialFactory
	{
		Material? CreateMaterial(IXmlDataMaterial xmlMaterial, DocumentItem parent);
	}
}