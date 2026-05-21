namespace KG.MES.Shared.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		public string Title { get; }
		public bool Visible { get; set; } = true;
		public string? DisplayFormat { get; set; }
		public int Order { get; set; }
		public bool IsBadge { get; set; }
		public string? BadgeProperty { get; set; }
		public string? BadgeGroup { get; set; }
		public int Width { get; set; }
		public string? CommentField { get; set; }

		public ColumnAttribute(string title)
		{
			Title = title;
		}
	}
}