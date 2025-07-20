using BankAccounts.Shared.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using BankAccounts.AppplicationData.Records;

namespace BankAccounts.Records
{
    public class AccountEntity
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }

        // Foreign key
        public int UserId { get; set; }

        // Navigation property
        public UserEntity User {  get; set; }

    }
}
