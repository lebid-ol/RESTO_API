using BankAccounts.Records;
using BankAccounts.Repositories;
using BankAccounts.RequestModel;

namespace BankAccounts.Services
{
    public static class AccountService
    {
        public static Account AddAccount(AccountRequest accountRequest)
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

            var accounts = new List<Account>()
            {
                account
            };

            AccountsRepository.AddAcountRecord(accounts);

            return account;
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

            return findAllAccount;

        }

        public static Account UpdateAccount(int id, UpdateAccountRequets requets)
        {
            var updatedAccount = new Account()
            {
                Id = id,
                AccountEmail = requets.AccountEmail,
                AccountName = requets.AccountName,
                UserLastName = requets.UserLastName,   
                UserName = requets.UserName,
            };


            return AccountsRepository.UpdateAccountRecord(updatedAccount);
        }

        public static void DeleteAccount(int id)
        {
            AccountsRepository.DeleteAccountFromData(id);
        }

        internal static Account GetAccountByName(string name)
        {
           return AccountsRepository.GetAccountByName(name);
        }
    }
}
