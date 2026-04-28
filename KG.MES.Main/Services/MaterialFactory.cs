using XmlOrderReader.Web.Interfaces;
using XmlOrderReader.Web.Models;

namespace XmlOrderReader.Web.Services
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