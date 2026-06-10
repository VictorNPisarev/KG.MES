using KG.MES.App.Main.Wasm.Interfaces;
using KG.MES.App.Main.Wasm.Models;

namespace KG.MES.App.Main.Wasm.Services
{
	public class MaterialFactory : IMaterialFactory
	{
		private readonly MaterialTypeConfigService _materialTypeConfigService;

		public MaterialFactory(MaterialTypeConfigService materialTypeConfigService)
		{
			_materialTypeConfigService = materialTypeConfigService;
		}

		public Material? CreateMaterial(IXmlDataMaterial xmlMaterial, DocumentItem parent)
		{
			return Material.FromXmlDataMaterial(xmlMaterial, parent, _materialTypeConfigService);
		}
	}
}