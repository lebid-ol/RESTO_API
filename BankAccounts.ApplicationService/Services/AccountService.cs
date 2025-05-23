﻿using BankAccounts.Repositories;
using BankAccounts.Shared.Models;

namespace BankAccounts.Services
{
    public interface IAccountService
    {
        Account AddAccount(Account accountRequest);
        Task<Account> GetAccount(string id);
        Task<List<Account>> GetAccounts();
        Task<Account> UpdateAccount(UpdateAccount requets);
        Task DeleteAccount(string id);
        Task<List<Account>> GetAllUserAccounts(string ownerId);
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

        

        public Task<Account> GetAccount(string id)
        {
           
            var findAccount = _accountsRepository.GetOneAccountFromData(id);
       
            return findAccount;

        }

        public Task<List<Account>> GetAccounts()
        {
            var findAllAccount = _accountsRepository.GetAllAccountsFromData();

            
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
