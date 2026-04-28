using System.Xml.Serialization;
using XmlOrderReader.Web.Interfaces;

namespace XmlOrderReader.Web.Models.Xml
{
	[XmlType("it_contact")]
	public class XmlContactInfo
	{
		[XmlElement("type")]
		public string? ContactType { get; set; }
		
		[XmlElement("connection_sequence")]
		public string? Value { get; set; }
		
		[XmlElement("comment")]
		public string? Comment { get; set; }
	}
}