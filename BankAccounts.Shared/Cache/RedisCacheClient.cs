using BankAccounts.Shared.Clients.CurrencyConver;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankAccounts.Shared.Cashe
{

    public interface IRedisCacheClient
    {
        Task<decimal> GetCadRate();
        
    }
    public class RedisCacheClient : IRedisCacheClient
    {
        private IDatabase _database;
        private  HttpClient _httpClient;
        private const string ApiKey = "d34c0f99a49d8015ce75d964e91c6d14";

        public RedisCacheClient(HttpClient httpClient)
        { 
           var connection = ConnectionMultiplexer.Connect("localhost:6379");
            _database = connection.GetDatabase();
            _httpClient = httpClient;
        }

        public async Task<decimal> GetCadRate()
        {
            const string cacheKey = "Cad_rate";
            var cachedValue = await _database.StringGetAsync(cacheKey);

            if (cachedValue.HasValue)
            {
                return decimal.Parse(cachedValue, CultureInfo.InvariantCulture);
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

            var newValue = rates.Rates["CAD"];
            await _database.StringSetAsync(cacheKey, newValue.ToString(CultureInfo.InvariantCulture));


            return newValue;
        }

    }
}
