using KG.MES.Masters.Components;
using KG.MES.Shared.Helpers;
using KG.MES.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpClient<ProductionApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

await LoadDataAsync(app.Services, app.Environment);

app.Run();

async Task LoadDataAsync(IServiceProvider services, IWebHostEnvironment env)
{
	using var scope = services.CreateScope();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

	//Загрузка конфига для выделения цветом полей-ярлыков
	try
	{
		var baseConfig = Path.Combine(env.ContentRootPath, "..", "KG.MES.Shared", "Config", "BadgeStyles.Base.json");
		var appConfig = Path.Combine(env.ContentRootPath, "Config", "BadgeStyles.json");

		BadgeHelper.LoadConfig(baseConfig, appConfig);

		logger.LogInformation("Badges config loaded successfully");
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "Failed to load badges config");
	}
}