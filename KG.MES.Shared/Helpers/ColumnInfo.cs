namespace KG.MES.Shared.Helpers
{
	public class ColumnInfo
	{
		public string PropertyName { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string? Format { get; set; }
		public bool IsBadge { get; set; }
		public string? BadgeProperty { get; set; } // если IsBadge и значение берется из другого свойства
	}
}