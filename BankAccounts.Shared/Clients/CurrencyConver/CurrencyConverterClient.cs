using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private const string ApiKey = "d34c0f99a49d8015ce75d964e91c6d14";

        public CurrencyConverterClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetCADRates()
        {
            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/v1/latest?access_key={ApiKey}&base=EUR&symbols=CAD");

            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch exchange rate. Error: {jsonString}");
            }

            var rates = JsonSerializer.Deserialize<ExchangeRatesResponse>(jsonString);

            if (rates == null || !rates.Success || !rates.Rates.ContainsKey("CAD"))
                throw new Exception("Invalid response from currency API.");

            return rates.Rates["CAD"];
        }
    }
}
