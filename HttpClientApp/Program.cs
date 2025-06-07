// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HttpClientApp;



var httpClient = new HttpClient();

var requestMessage = new HttpRequestMessage();

requestMessage.Headers.Add("Hello", "Sergei");
requestMessage.RequestUri = new Uri("https://localhost:7112/api/accounts");
requestMessage.Method = new HttpMethod("GET");


// separate service
var response = await httpClient.SendAsync(requestMessage);

var test = response.StatusCode;

var jsonString = await response.Content.ReadAsStringAsync();
var bankResponse = JsonSerializer.Deserialize<List<BankAccountResponse>>(jsonString);


// other way
var responseOther = await httpClient.GetAsync("https://localhost:7112/api/accounts");
var jsonString2 = await responseOther.Content.ReadAsStringAsync();
var bankResponse2 = JsonSerializer.Deserialize<List<BankAccountResponse>>(jsonString2);

var createUserRequest = new CreateUserRequest()
{
    UserName = "Test",
    UserLastName = "Test",
    BillingAddress = "Test",
    DateOfBirth = "1986-07-07",
    Email = "Test",
    Gender = "Man",
    PhoneNumber = "Test"
};

var stringRequest = JsonSerializer.Serialize(createUserRequest);
var stringContent = new StringContent(stringRequest, Encoding.UTF8, "application/json");

var responsePost = await httpClient.PutAsync("https://localhost:7112/api/user", stringContent);

var respCreateUser = await responsePost.Content.ReadAsStringAsync();


foreach (var item in bankResponse)
{
    item.balance = 500000;
}

var newJsonString = JsonSerializer.Serialize(bankResponse);
Console.WriteLine(newJsonString);

// my homework
var responseRates = await httpClient.GetAsync("https://api.exchangeratesapi.io/v1/latest?access_key=d34c0f99a49d8015ce75d964e91c6d14&base=EUR&symbols=CAD");
var ratesToJsonString = await responseRates.Content.ReadAsStringAsync();
var bankResponseRates = JsonSerializer.Deserialize<ExchangeRatesResponse>(ratesToJsonString);
Console.WriteLine($"Курс EUR → CAD: {bankResponseRates.Rates["CAD"]}");