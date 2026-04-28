using KG.MES.Main.Interfaces;
using KG.MES.Main.Models;
using KG.MES.Main.Models.Xml;
using KG.MES.Main.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add our custom services
builder.Services.AddScoped<IXmlReaderService, XmlReaderService>();
builder.Services.AddHttpClient<I1CExportService, OneCExportService>();
builder.Services.AddHttpClient<ProductionApiService>();

builder.Services.AddSingleton<SortingIndicatorService>();
builder.Services.AddSingleton<MaterialTypeConfigService>();
builder.Services.AddSingleton<HolidayCalendarService>();
builder.Services.AddSingleton<IMaterialFactory, MaterialFactory>();
builder.Services.AddScoped<IDocumentItemFactory, DocumentItemFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await LoadDataAsync(app.Services, app.Environment);

app.Run();

async Task LoadDataAsync(IServiceProvider services, IWebHostEnvironment env)
{
	using var scope = services.CreateScope();
	var indicatorService = scope.ServiceProvider.GetRequiredService<SortingIndicatorService>();
	var materialTypeConfigService = scope.ServiceProvider.GetRequiredService<MaterialTypeConfigService>();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
	
	var indicatorsPath = Path.Combine(env.ContentRootPath, "Data", "sorting_indicators.json");
	var categoriesPath = Path.Combine(env.ContentRootPath, "Data", "indicator_categories.json");
	var materialTypeConfigPath = Path.Combine(env.ContentRootPath, "Data", "material_types_rules.json");
	
	try
	{
		await indicatorService.LoadAsync(indicatorsPath, categoriesPath);
		await materialTypeConfigService.LoadAsync(materialTypeConfigPath);

		logger.LogInformation("Indicators and categories loaded successfully");
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "Failed to load indicators and categories");
	}
}