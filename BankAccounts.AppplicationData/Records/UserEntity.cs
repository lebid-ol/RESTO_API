﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using static BankAccounts.Shared.Models.GenderType;

namespace BankAccounts.AppplicationData.Records
{
    public class UserEntity 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserLastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; } = "New York";
    }
}
