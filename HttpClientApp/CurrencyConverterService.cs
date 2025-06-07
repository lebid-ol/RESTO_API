using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientApp
{
    public interface ICurrencyConverterService
    {
        Task<decimal> ConvertEurToCad(decimal amount);
    }

    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "d34c0f99a49d8015ce75d964e91c6d14";

        public CurrencyConverterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertEurToCad(decimal amount)
        {
            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/v1/latest?access_key={ApiKey}&base=EUR&symbols=CAD");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch exchange rate.");

            var json = await response.Content.ReadAsStringAsync();
            var rates = JsonSerializer.Deserialize<ExchangeRatesResponse>(json);

            if (rates == null || !rates.Success || !rates.Rates.ContainsKey("CAD"))
                throw new Exception("Invalid response from currency API.");

            return amount * rates.Rates["CAD"];
        }
    }
}
