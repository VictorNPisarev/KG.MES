using KG.MES.Shared.Helpers;
using KG.MES.Shared.Models.Config;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.Supply.Components;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpClient<ProductionApiService>();
builder.Services.AddSingleton(LoadViewSettings());
builder.Services.AddSingleton<SupplyService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

await LoadDataAsync(app.Services, app.Environment);

app.Run();

async Task LoadDataAsync(IServiceProvider services, IWebHostEnvironment env)
{
	using var scope = services.CreateScope();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

	try
	{
		var baseConfig = Path.Combine(env.ContentRootPath, "..", "KG.MES.Shared", "Config", "BadgeStyles.Base.json");
		var appConfig = Path.Combine(env.ContentRootPath, "Config", "BadgeStyles.json");
		BadgeHelper.LoadConfig(baseConfig, appConfig);
		logger.LogInformation("Badges config loaded successfully");

		var supplyService = scope.ServiceProvider.GetRequiredService<SupplyService>();
		var conditions = await supplyService.GetConditionsAsync();
		foreach (SupplyCondition c in conditions)
		{
			BadgeHelper.RegisterStatusDisplayName(c.ConditionCode, c.DisplayName());
		}
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "Failed to load badges config");
	}
}

OrderViewSettings LoadViewSettings()
{
	var settingsPath = Path.Combine(builder.Environment.ContentRootPath, "Config", "orderViewSettings.json");
	if (File.Exists(settingsPath))
	{
		var json = File.ReadAllText(settingsPath);
		return JsonSerializer.Deserialize<OrderViewSettings>(json,
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new OrderViewSettings();
	}
	return new OrderViewSettings();
}