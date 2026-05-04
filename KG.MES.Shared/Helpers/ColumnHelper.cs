using System.Reflection;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Helpers
{
	public static class ColumnHelper
	{
		public static List<ColumnInfo> GetColumns<T>()
		{
			return typeof(T).GetProperties()
				.Select(p => new
				{
					Property = p,
					Attr = p.GetCustomAttribute<ColumnAttribute>()
				})
				.Where(x => x.Attr != null)
				.OrderBy(x => x.Attr!.Order)
				.Select(x => new ColumnInfo
				{
					PropertyName = x.Property.Name,
					Title = x.Attr!.Title,
					Format = x.Attr.DisplayFormat,
					IsBadge = x.Attr.IsBadge
				})
				.ToList();
		}

		public static string? GetFormattedValue(object obj, ColumnInfo column)
		{
			var property = obj.GetType().GetProperty(column.PropertyName);
			var value = property?.GetValue(obj);

			if (value == null) return null;

			if (!string.IsNullOrEmpty(column.Format))
			{
				if (value is DateTime dt)
					return dt.ToString(column.Format);
				if (value is double d)
					return d.ToString(column.Format);
				if (value is decimal m)
					return m.ToString(column.Format);
			}

			return value.ToString();
		}

		public static List<ColumnSetting> GetDefaultSettings<T>()
		{
			var settings = typeof(T).GetProperties()
				.Select(p => new
				{
					Property = p,
					Attr = p.GetCustomAttribute<ColumnAttribute>()
				})
				.Where(x => x.Attr != null)
				.Select(x => new ColumnSetting
				{
					PropertyName = x.Property.Name,
					Visible = x.Attr!.Visible,
					Order = x.Attr.Order,
					Width = 0
				})
				.ToList();

			// Перенумеровываем подряд, начиная с 0
			var ordered = settings.OrderBy(s => s.Order).ToList();
			for (int i = 0; i < ordered.Count; i++)
				ordered[i].Order = i;

			return settings;
		}
	}
}