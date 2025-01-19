using BankAccounts.Records;
using BankAccounts.Repositories;
using BankAccounts.RequestModel;
using BankAccounts.Shared.Models.Request;

namespace BankAccounts.Services
{
    public class AccountService
    {
        private readonly AccountsRepository _accountsRepository;

        public AccountService()
        {
            _accountsRepository = new AccountsRepository();
        }

        public Account AddAccount(AccountRequest accountRequest)
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

            _accountsRepository.AddAcountRecord(accounts);

            return account;
        }

        private int GetNextAccountID() 
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

        public Account? GetAccount(int id)
        {
            var findAccount = _accountsRepository.GetOneAccountFromData(id);
                
            if (findAccount == null)
            {
                return null;
            }

            return findAccount;

        }

        public List <Account> GetAccounts()
        {
            var findAllAccount = _accountsRepository.GetAllAccountsFromData();

            return findAllAccount;

        }

        public Account UpdateAccount(int id, UpdateAccountRequets requets)
        {
            var updatedAccount = new Account()
            {
                Id = id,
                AccountEmail = requets.AccountEmail,
                AccountName = requets.AccountName,
                UserLastName = requets.UserLastName,   
                UserName = requets.UserName,
            };


            return _accountsRepository.UpdateAccountRecord(updatedAccount);
        }

        public void DeleteAccount(int id)
        {
            _accountsRepository.DeleteAccountFromData(id);
        }

        public Account GetAccountByName(string name)
        {
           return _accountsRepository.GetAccountByName(name);
        }
    }
}
