using System.Text;
using System.Text.Json;
using XmlOrderReader.Web.Models;
using KG.MES.Shared.Models.Dto;

namespace XmlOrderReader.Web.Services
{
	public class ProductionApiService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<ProductionApiService> _logger;
		private readonly IConfiguration _configuration;

		public ProductionApiService(
			HttpClient httpClient,
			ILogger<ProductionApiService> logger,
			IConfiguration configuration)
		{
			_httpClient = httpClient;
			_logger = logger;
			_configuration = configuration;
		}

		private string BaseUrl => _configuration["ProductionApi:BaseUrl"] ?? "http://localhost:5000/api";

		private int TimeoutSeconds => _configuration.GetValue<int>("ProductionApi:TimeoutSeconds", 30);

		private int RetryCount => _configuration.GetValue<int>("ProductionApi:RetryCount", 3);

		/// <summary>
		/// POST запись нового заказ в бд
		/// </summary>
		/// <param name="order"></param>
		/// <param name="dto"></param>
		/// <returns></returns>
		public async Task<bool> ExportToProductionAsync(DocumentData order, ProductionOrderExportDto dto)
		{
			var retries = 0;

			while (retries < RetryCount)
			{
				try
				{
					using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutSeconds));

					var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});

					var content = new StringContent(json, Encoding.UTF8, "application/json");

					var response = await _httpClient.PostAsync($"{BaseUrl}/orders/create", content, cts.Token);

					if (response.IsSuccessStatusCode)
					{
						_logger.LogInformation("Order {OrderNumber} sent to production successfully", order.DocumentNumber);
						return true;
					}

					var error = await response.Content.ReadAsStringAsync(cts.Token);
					_logger.LogWarning("Attempt {Retry}/{RetryCount} failed: {StatusCode} - {Error}",
						retries + 1, RetryCount, response.StatusCode, error);
				}
				catch (TaskCanceledException)
				{
					_logger.LogWarning("Attempt {Retry}/{RetryCount} timeout after {Timeout} seconds",
						retries + 1, RetryCount, TimeoutSeconds);
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Attempt {Retry}/{RetryCount} failed", retries + 1, RetryCount);
				}

				retries++;

				if (retries < RetryCount)
				{
					await Task.Delay(1000 * retries); // экспоненциальная задержка
				}
			}

			_logger.LogError("Failed to send order {OrderNumber} to production after {RetryCount} attempts",
				order.DocumentNumber, RetryCount);

			return false;
		}

		/// <summary>
		/// GET заказов
		/// </summary>
		/// <param name="status"></param>
		/// <param name="number"></param>
		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortOrder"></param>
		/// <returns></returns>
		public async Task<PaginatedResponse<ProductionOrderDto>> GetOrdersAsync(
			string? status = null,
			string? number = null,
			int page = 1,
			int limit = 50,
			string? sortBy = null,
			string? sortOrder = null)
		{
			try
			{
				// Поиск по номеру
				if (!string.IsNullOrEmpty(number))
				{
					var orderUrl = $"{BaseUrl}/orders/{Uri.EscapeDataString(number)}";
					var order = await _httpClient.GetFromJsonAsync<ProductionOrderDto>(orderUrl);

					return new PaginatedResponse<ProductionOrderDto>
					{
						Data = order != null ? new List<ProductionOrderDto> { order } : new(),
						Pagination = new PaginationInfo { Page = 1, Limit = 1, Total = order != null ? 1 : 0, Pages = 1 }
					};
				}

				// Список с пагинацией и сортировкой
				var endpoint = !string.IsNullOrEmpty(status)
					? $"orders/{Uri.EscapeDataString(status)}"
					: "orders/all";

				var queryParams = new List<string>
			{
				$"page={page}",
				$"limit={limit}"
			};

				if (!string.IsNullOrEmpty(sortBy))
					queryParams.Add($"sortBy={Uri.EscapeDataString(sortBy)}");

				if (!string.IsNullOrEmpty(sortOrder))
					queryParams.Add($"sortOrder={Uri.EscapeDataString(sortOrder)}");

				var listUrl = $"{BaseUrl}/{endpoint}?" + string.Join("&", queryParams);

				_logger.LogInformation("Fetching orders: {Url}", listUrl);

				var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<ProductionOrderDto>>(listUrl);
				return response ?? new PaginatedResponse<ProductionOrderDto>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching orders from API");
				return new PaginatedResponse<ProductionOrderDto>();
			}
		}

		public async Task<bool> TestConnectionAsync()
		{
			try
			{
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				var response = await _httpClient.GetAsync($"{BaseUrl}/health", cts.Token);
				return response.IsSuccessStatusCode;
			}
			catch
			{
				return false;
			}
		}
	}
}