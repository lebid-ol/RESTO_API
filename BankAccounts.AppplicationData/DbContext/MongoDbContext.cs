using MongoDB.Driver;
using BankAccounts.Records;
using BankAccounts.AppplicationData.Records;

namespace BankAccounts.AppplicationData.DbContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext() 
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            _database = client.GetDatabase("bank_accounts");
        }

        public IMongoCollection<AccountEntity> Accounts => _database.GetCollection<AccountEntity>("accounts");
        public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("users");
    }
}
