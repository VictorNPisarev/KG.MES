using KG.MES.Shared.Models.Dto;
using Microsoft.Extensions.Logging;

namespace KG.MES.Shared.Services
{
	public class SupplyService
	{
		private readonly ProductionApiService _api;
		private readonly ILogger<SupplyService> _logger;

		private List<SupplyTypeDto>? _cachedTypes;
		private List<SupplyConditionDto>? _cachedConditions;

		public SupplyService(ProductionApiService api, ILogger<SupplyService> logger)
		{
			_api = api;
			_logger = logger;
		}

		public async Task<List<SupplyTypeDto>> GetTypesAsync()
		{
			if (_cachedTypes != null) return _cachedTypes;

			try
			{
				_cachedTypes = await _api.GetSupplyTypesAsync();
				return _cachedTypes ?? [];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading supply types");
				return [];
			}
		}

		public async Task<List<SupplyConditionDto>> GetConditionsAsync()
		{
			if (_cachedConditions != null) return _cachedConditions;

			try
			{
				_cachedConditions = await _api.GetSupplyConditionsAsync();
				return _cachedConditions ?? [];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading supply conditions");
				return [];
			}
		}
	}
}