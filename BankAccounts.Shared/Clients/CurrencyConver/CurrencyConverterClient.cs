using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace BankAccounts.Shared.Clients.CurrencyConver
{
    public interface ICurrencyConverterClient
    {
        Task<decimal> GetCADRates();
    }

    public class CurrencyConverterClient : ICurrencyConverterClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string ApiKey = "d34c0f99a49d8015ce75d964e91c6d14";
      

        public CurrencyConverterClient(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<decimal> GetCADRates()
        {
            const string cacheKey = "Cad_rate";

            if (_cache.TryGetValue(cacheKey, out decimal cachedRate))
            {
                return cachedRate;
            }

            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/v1/latest?access_key={ApiKey}&base=EUR&symbols=CAD");

            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch exchange rate. Error: {jsonString}");
            }

            var rates = JsonSerializer.Deserialize<ExchangeRatesResponse>(jsonString);

            if (rates == null || !rates.Success || !rates.Rates.ContainsKey("CAD"))
                throw new Exception("Invalid response from currency API.");

            decimal cadRate = rates.Rates["CAD"];


            _cache.Set(cacheKey, cadRate, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return cadRate;    
        }
    }
}
