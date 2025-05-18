// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Text.Json;
using HttpClientApp;



var httpClient = new HttpClient();


var requestMessage = new HttpRequestMessage();

requestMessage.Headers.Add("Hello", "Sergei");
requestMessage.RequestUri = new Uri("https://localhost:7112/api/accounts");
requestMessage.Method = new HttpMethod("GET");



// separate service
var response = await httpClient.SendAsync(requestMessage);

var jsonString = await response.Content.ReadAsStringAsync();
var bankResponse = JsonSerializer.Deserialize<List<BankAccountResponse>>(jsonString);

foreach (var item in bankResponse)
{
    item.balance = 500000;
}

var newJsonString = JsonSerializer.Serialize(bankResponse);
Console.WriteLine(newJsonString);