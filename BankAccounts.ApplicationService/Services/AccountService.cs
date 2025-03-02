using BankAccounts.Repositories;
using BankAccounts.Shared.Models;

namespace BankAccounts.Services
{
    public interface IAccountService
    {
        Account AddAccount(Account accountRequest);
        Account? GetAccount(int id);
        Task<List<Account>> GetAccounts();
        Account UpdateAccount(UpdateAccount requets);
        void DeleteAccount(int id);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountsRepository;

        public AccountService(IAccountRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
   
        }

        public Account AddAccount(Account account)
        {
            account.Balance = 100;
            account.CreatedDate = DateTime.Now;

            var createdAccount = _accountsRepository.AddAcountRecord(account);

            return createdAccount;
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

        public Task<List <Account>> GetAccounts()
        {
            var findAllAccount = _accountsRepository.GetAllAccountsFromData();

            return findAllAccount;

        }

        public Account UpdateAccount(UpdateAccount updatedAccount)
        {
            updatedAccount.UpdateDate = DateTime.Now;

            var account = _accountsRepository.UpdateAccountRecord(updatedAccount);
            return account;
        }

        public void DeleteAccount(int id)
        {
            _accountsRepository.DeleteAccountFromData(id);
        }
    }
    
}
