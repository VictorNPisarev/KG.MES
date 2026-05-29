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
		builder.Configuration
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddEnvironmentVariables();

		string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

		if (connectionString is not null)
		{
			connectionString = ConnectionStringRegex().Replace(connectionString, match =>
			{
				string variable = match.Groups[1].Value;
				return Environment.GetEnvironmentVariable(variable) ?? match.Value;
			});
		}

		return connectionString;
	}
}