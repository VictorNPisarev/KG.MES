using System.Text.Json;
using KG.MES.App.Main.Wasm;
using KG.MES.App.Main.Wasm.Interfaces;
using KG.MES.App.Main.Wasm.Models;
using KG.MES.App.Main.Wasm.Models.Xml;
using KG.MES.App.Main.Wasm.Services;
using KG.MES.Shared.Helpers;
using KG.MES.Shared.Interfaces;
using KG.MES.Shared.Models.Config;
using KG.MES.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Настройка HttpClient для API
builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]
		?? builder.HostEnvironment.BaseAddress)
});

// Регистрация сервисов
builder.Services.AddScoped<IXmlReaderService, XmlReaderService>();
builder.Services.AddScoped<I1CExportService, OneCExportService>();
builder.Services.AddScoped<ProductionApiService>();
builder.Services.AddSingleton<SortingIndicatorService>();
builder.Services.AddSingleton<MaterialTypeConfigService>();
builder.Services.AddSingleton<HolidayCalendarService>();
builder.Services.AddSingleton<IMaterialFactory, MaterialFactory>();
builder.Services.AddScoped<IDocumentItemFactory, DocumentItemFactory>();
// OrderViewSettings регистрируем как Singleton, но заполним позже
builder.Services.AddSingleton<OrderViewSettings>();
builder.Services.AddSingleton<SupplyService>();
builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
builder.Services.AddScoped<ISocketService, SocketService>();

var host = builder.Build();

// ===== Загрузка всех конфигов АСИНХРОННО =====
await LoadAllConfigsAsync(host.Services);

await host.RunAsync();

async Task LoadAllConfigsAsync(IServiceProvider services)
{
	// Используем HttpClient из DI (с правильным BaseAddress)
	var httpClient = services.GetRequiredService<HttpClient>();
	var loggerFactory = services.GetService<ILoggerFactory>();
	var logger = loggerFactory?.CreateLogger("Program");

	using var scope = services.CreateScope();

	// 1. Загрузка индикаторов и категорий
	var indicatorService = scope.ServiceProvider.GetRequiredService<SortingIndicatorService>();
	var materialTypeConfigService = scope.ServiceProvider.GetRequiredService<MaterialTypeConfigService>();

	try
	{
		var indicatorsJson = await httpClient.GetStringAsync("Data/sorting_indicators.json");
		var categoriesJson = await httpClient.GetStringAsync("Data/indicator_categories.json");
		var materialTypeConfigJson = await httpClient.GetStringAsync("Data/material_types_rules.json");

		await indicatorService.LoadAsync(indicatorsJson, categoriesJson);
		await materialTypeConfigService.LoadAsync(materialTypeConfigJson);

		logger?.LogInformation("Indicators and categories loaded successfully");
	}
	catch (Exception ex)
	{
		logger?.LogError(ex, "Failed to load indicators and categories");
	}

	// 2. Загрузка конфига бейджей
	try
	{
		var baseConfigJson = await httpClient.GetStringAsync("Config/BadgeStyles.Base.json");
		var appConfigJson = await httpClient.GetStringAsync("Config/BadgeStyles.json");

		BadgeHelper.LoadConfig(baseConfigJson, appConfigJson);

		logger?.LogInformation("Badges config loaded successfully");
	}
	catch (Exception ex)
	{
		logger?.LogError(ex, "Failed to load badges config");
	}

	// 3. Загрузка настроек отображения (OrderViewSettings)
	try
	{
		var settingsJson = await httpClient.GetStringAsync("Config/orderViewSettings.json");
		var settings = JsonSerializer.Deserialize<OrderViewSettings>(settingsJson,
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new OrderViewSettings();

		// Заменяем зарегистрированный Singleton на загруженный
		var settingsService = services.GetRequiredService<OrderViewSettings>();
		// Копируем свойства (или используйте другой подход)
		CopyProperties(settings, settingsService);
	}
	catch (Exception ex)
	{
		logger?.LogError(ex, "Failed to load OrderViewSettings");
	}
}

// Вспомогательный метод для копирования свойств
void CopyProperties(OrderViewSettings source, OrderViewSettings target)
{
	var properties = typeof(OrderViewSettings).GetProperties();
	foreach (var prop in properties)
	{
		if (prop.CanWrite)
		{
			prop.SetValue(target, prop.GetValue(source));
		}
	}
}