using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using KG.MES.Server.Data;
using KG.MES.Server.Hubs;
using KG.MES.Server.Services;
using KG.MES.Server.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем DbContext
var connectionString = GetConnectionString(builder);
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(connectionString));

// Регистрация API сервисов
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IWorkplaceService, WorkplaceService>();

// Добавляем контроллеры с настройкой JSON (игнорировать циклы)
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.WriteIndented = true;
	});

// Добавляем SignalR
builder.Services.AddSignalR();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка CORS для доступа с любых устройств
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapHub<NotificationHub>("/notificationHub");

// Инициализация NotificationHelper (после app.Build())
var hubContext = app.Services.GetRequiredService<IHubContext<NotificationHub>>();
NotificationHelper.Initialize(hubContext);

app.MapControllers();

app.Run();

partial class Program
{
	[GeneratedRegex(@"\${(\w+)}")]
	private static partial Regex ConnectionStringRegex();

	private static string? GetConnectionString(WebApplicationBuilder builder)
	{
		// Загружаем .env файл
		var envFile = Path.Combine(Directory.GetCurrentDirectory(), ".env");
		var envVars = new Dictionary<string, string>();

		if (File.Exists(envFile))
		{
			foreach (var line in File.ReadAllLines(envFile))
			{
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
					continue;

				var parts = line.Split('=', 2);
				if (parts.Length == 2)
				{
					envVars[parts[0].Trim()] = parts[1].Trim();
				}
			}
		}

		// Собираем строку подключения напрямую
		var host = envVars.GetValueOrDefault("DB_HOST", "localhost");
		var port = envVars.GetValueOrDefault("DB_PORT", "5432");
		var database = envVars.GetValueOrDefault("DB_NAME", "KgMes");
		var username = envVars.GetValueOrDefault("DB_USER", "postgres");
		var password = envVars.GetValueOrDefault("DB_PASSWORD", "");

		var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

		Console.WriteLine($"connectionString: {connectionString}");
		return connectionString;
	}
}