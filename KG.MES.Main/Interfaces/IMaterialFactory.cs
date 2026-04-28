using XmlOrderReader.Web.Models;

namespace XmlOrderReader.Web.Interfaces
{
	public interface IMaterialFactory
	{
		Material? CreateMaterial(IXmlDataMaterial xmlMaterial, DocumentItem parent);
	}
}