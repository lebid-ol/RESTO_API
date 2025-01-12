using BankAccounts.Records;
using BankAccounts.Repositories;
using BankAccounts.RequestModel;
using CsvHelper;
using System.Globalization;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace BankAccounts.Services
{
    public static class AccountService
    {
        public static void AddAccount(AccountRequest accountRequest)
        {

            var account = new Account()
            {
                AccountEmail = accountRequest.Email,
                AccountName = accountRequest.AccountName,
                Balance = 0,
                CreatedDate = DateTime.Now,
                Id = GetNextAccountID(),
                UserLastName = accountRequest.AccountHolderLastName,
                UserName = accountRequest.AccountHolderName
            };

            var accounts = new List<Account>();

            accounts.Add(account);

            AccountsRepository.AddAcountRecord(accounts);

        }

        private static int GetNextAccountID() 
        {
            if (!File.Exists("accountId.txt"))
            {
                File.WriteAllText("accountId.txt", "1");

                return 1;
            }
            else
            {
                var id = File.ReadAllText("accountId.txt");
                var intId = int.Parse(id);
                var nextId = intId + 1;
                File.WriteAllText("accountId.txt", nextId.ToString());
                return nextId; 
            }

        }

        public static Account? GetAccount(int id)
        {
            var findAccount = AccountsRepository.GetOneAccountFromData(id);
                
            if (findAccount == null)
            {
                return null;
            }

            return findAccount;

        }

        public static List <Account> GetAccounts()
        {
            var findAllAccount = AccountsRepository.GetAllAccountsFromData();

            if (findAllAccount == null)
            {
                return null;
            }

            return findAllAccount;

        }

    }
}
