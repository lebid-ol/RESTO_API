using MongoDB.Driver;
using BankAccounts.Records;
using BankAccounts.AppplicationData.Records;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.Extensions.Options;


namespace BankAccounts.AppplicationData.DbContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private AzureSettingsOptions _azureSettings;

        public MongoDbContext(IOptions<AzureSettingsOptions> azureOptions) 
        {
            _azureSettings = azureOptions.Value;


            var client = new MongoClient(_azureSettings.MongoDBConnectionString);
            _database = client.GetDatabase("bank_accounts");
        }

        public IMongoCollection<AccountEntity> Accounts => _database.GetCollection<AccountEntity>(MyConstants.AccountsTable);
        public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>(MyConstants.UserTable);
    }

    public class AzureSettingsOptions
    {
        public const string SectionName = "Azure";
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
        public int LifeSpan { get; set; }
        public List<string> TableNames { get; set; }
        public string MongoDBConnectionString { get; set; }
    }

    public class MyOptions
    {
        public const string SectionName = "HelloSection";
        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public class MyConstants
    {
        public const string UserTable = "users";
        public const string AccountsTable = "account";
    }
}
