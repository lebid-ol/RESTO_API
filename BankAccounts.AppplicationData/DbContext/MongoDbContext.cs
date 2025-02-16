using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using BankAccounts.Records;

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
    }
}
