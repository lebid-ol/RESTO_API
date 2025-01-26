using BankAccounts.API.DI_test;
using BankAccounts.Records;
using BankAccounts.Repositories;
using BankAccounts.RequestModel;
using BankAccounts.Shared.Models.Request;

namespace BankAccounts.Services
{
    public interface IAccountService
    {
        Account AddAccount(AccountRequest accountRequest);
        Account? GetAccount(int id);
        List<Account> GetAccounts();
        Account UpdateAccount(int id, UpdateAccountRequets requets);
        void DeleteAccount(int id);
        Account GetAccountByName(string name);

    }

    public class AccountServiceV2 : IAccountService
    {
        public Account AddAccount(AccountRequest accountRequest)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Account? GetAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public Account UpdateAccount(int id, UpdateAccountRequets requets)
        {
            throw new NotImplementedException();
        }
    }

    public class AccountService : IAccountService
    {
        private readonly AccountsRepository _accountsRepository;

        public AccountService(AccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
   
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
