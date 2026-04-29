namespace KG.MES.Shared.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		public string Title { get; }
		public bool Visible { get; set; } = true;
		public string? DisplayFormat { get; set; }
		public int Order { get; set; }

		public ColumnAttribute(string title)
		{
			Title = title;
		}
	}
}