using System.Text.RegularExpressions;
using KG.MES.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем SignalR
builder.Services.AddSignalR();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрируем DbContext
var connectionString = GetConnectionString(builder);
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
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
		var database = envVars.GetValueOrDefault("DB_NAME", "WorkshopMES");
		var username = envVars.GetValueOrDefault("DB_USER", "postgres");
		var password = envVars.GetValueOrDefault("DB_PASSWORD", "");

		var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

		Console.WriteLine($"connectionString: {connectionString}");
		return connectionString;
	}
}