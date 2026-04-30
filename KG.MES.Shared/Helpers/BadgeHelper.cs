using System.Text.Json;
using System.Text.Json.Serialization;

namespace KG.MES.Shared.Helpers
{
	public static class BadgeHelper
	{
		private static Dictionary<string, string> _statusStyles = new();
		private static Dictionary<string, string> _booleanStyles = new();
		private static string _defaultStatus = "bg-light text-dark";
		private static string _defaultBoolean = "bg-light text-dark";

		public static void LoadConfig(string baseConfigPath, string? appConfigPath = null)
		{
			// Загружаем базовый
			if (File.Exists(baseConfigPath))
			{
				var baseJson = File.ReadAllText(baseConfigPath);
				MergeConfig(baseJson);
			}

			// Поверх накладываем специфичный для приложения
			if (!string.IsNullOrEmpty(appConfigPath) && File.Exists(appConfigPath))
			{
				var appJson = File.ReadAllText(appConfigPath);
				MergeConfig(appJson);
			}
		}

		private static void MergeConfig(string json)
		{
			var config = JsonSerializer.Deserialize<BadgeConfig>(json);

			if (config?.Statuses != null)
			{
				foreach (var kvp in config.Statuses)
					_statusStyles[kvp.Key.ToLower()] = kvp.Value;
			}

			if (config?.Booleans != null)
			{
				foreach (var kvp in config.Booleans)
					_booleanStyles[kvp.Key] = kvp.Value;
			}
		}

		public static string GetBadgeClass(string? value, bool isBoolean = false)
		{
			if (string.IsNullOrEmpty(value))
				return isBoolean ? _defaultBoolean : _defaultStatus;

			if (isBoolean)
				return _booleanStyles.GetValueOrDefault(value, _defaultBoolean);

			return _statusStyles.GetValueOrDefault(value.ToLower(), _defaultStatus);
		}

		public static string GetFormattedBadgeValue(object obj, ColumnInfo column)
		{
			var propertyName = column.BadgeProperty ?? column.PropertyName;
			var property = obj.GetType().GetProperty(propertyName);
			var value = property?.GetValue(obj);

			return value switch
			{
				bool b => b ? "Да" : "Нет",
				null => "—",
				_ => value.ToString() ?? "—"
			};
		}

		private class BadgeConfig
		{
			[JsonPropertyName("statuses")]
			public Dictionary<string, string> Statuses { get; set; } = new();

			[JsonPropertyName("booleans")]
			public Dictionary<string, string> Booleans { get; set; } = new();

			[JsonPropertyName("defaultStatus")]
			public string DefaultStatus { get; set; } = "bg-light text-dark";

			[JsonPropertyName("defaultBoolean")]
			public string DefaultBoolean { get; set; } = "bg-light text-dark";
		}
	}
}