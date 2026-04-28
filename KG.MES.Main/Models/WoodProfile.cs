using System.Xml.Serialization;
using XmlOrderReader.Web.Models.Xml;

namespace XmlOrderReader.Web.Models
{
	public class WoodProfile
	{
		/// <summary>
		/// Компонент, к которому относится профиль (рама, створка,...)
		/// </summary>
		public string? Component { get; set; }
		
		/// <summary>
		/// Профильная система
		/// </summary>
		public string? ProfileGroup { get; set; }
		
		/// <summary>
		/// Описание профиля
		/// </summary>
		public string? Description { get; set; }
		
		/// <summary>
		/// Список бруса
		/// </summary>
		public List<ProfileDetailData> profileDetailDatas { get; set; } = new();

		public WoodProfile () {}

		public WoodProfile (XmlWoodProfile xmlWoodProfile)
		{
			Component = xmlWoodProfile.Component;
			ProfileGroup = xmlWoodProfile.ProfileGroup;
			Description = xmlWoodProfile.Description;
			
			foreach(var xmlProfileDetail in xmlWoodProfile.xmlProfileDetailDatas)
			{
				profileDetailDatas.Add(new(xmlProfileDetail, this)); 
			}
		}
	}
}