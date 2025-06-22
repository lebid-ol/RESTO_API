using BankAccounts.Repositories;
using BankAccounts.Shared.Cashe;
using BankAccounts.Shared.Clients.CurrencyConver;
using BankAccounts.Shared.Models;
using MongoDB.Driver;

namespace BankAccounts.Services
{
    public interface IAccountService
    {
        Task<Account> AddAccount(Account accountRequest);
        Task<Account> GetAccount(string id);
        Task<List<Account>> GetAccounts();
        Task<Account> UpdateAccount(UpdateAccount requets);
        Task DeleteAccount(string id);
        Task<List<Account>> GetAllUserAccounts(string ownerId);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountsRepository;
        private readonly ICurrencyConverterClient _currencyConverter;
        private readonly IRedisCacheClient _redisCacheClient;

        public AccountService(IAccountRepository accountsRepository, 
            ICurrencyConverterClient currencyConverter, IRedisCacheClient cacheClient)
        {
            _accountsRepository = accountsRepository;
            _currencyConverter = currencyConverter;
            _redisCacheClient = cacheClient;
        }

        public async Task<Account> AddAccount(Account account)
        {
            account.Balance = 100;
            account.CreatedDate = DateTime.Now;
            var rate = await _redisCacheClient.GetCadRate();
            account.BalanceInEuro = account.Balance * rate;

            var createdAccount = _accountsRepository.AddAcountRecord(account);

            return createdAccount;
        }

        

        public async Task<Account> GetAccount(string id)
        {
           
            var findAccount = await _accountsRepository.GetOneAccountFromData(id);
            findAccount.BalanceInEuro = findAccount.Balance * await _currencyConverter.GetCADRates();


            return findAccount;

        }

        public async Task<List<Account>> GetAccounts()
        {
            var findAllAccount = await _accountsRepository.GetAllAccountsFromData();

            var rate = await _currencyConverter.GetCADRates();

            foreach (var account in findAllAccount)
            {
                account.BalanceInEuro = account.Balance * rate;
            }

            return findAllAccount;

        }

        public Task<Account> UpdateAccount(UpdateAccount updatedAccount)
        {
            updatedAccount.UpdateDate = DateTime.Now;

            var account = _accountsRepository.UpdateAccountRecord(updatedAccount);
            return account;
        }

        public Task DeleteAccount(string id)
        {
            return  _accountsRepository.DeleteAccountFromData(id);
        }

        public Task<List<Account>> GetAllUserAccounts(string ownerId)
        {
            return _accountsRepository.GetAllAccountsByOwnerId(ownerId);
        }
    }
    
}
