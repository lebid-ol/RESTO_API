using System;
using BankAccounts.API.DI_test;
using BankAccounts.AppplicationData.DbContext;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.RequestModel;
using BankAccounts.ResponseModels;
using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
             _accountService = accountService;
        }

        // GET: api/<AccountsController>
        [HttpGet]
        public ActionResult<List<AccountResponse>> GetAllAccounts()
        {

            try
            {
                var allAccounts = _accountService.GetAccounts();

                return Ok(allAccounts);

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DontExistException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponse>> GetAccountById([FromRoute] string id)
        {
            try
            {
                var account = await _accountService.GetAccount(id);

                var response = new AccountResponse() 
                { 
                    Id = account.Id,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    Balance = account.Balance   
                };

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
    
                return NotFound(ex.Message);
            }
            catch (DontExistException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<AccountsController>
        [HttpPost]
        public ActionResult<AccountResponse> CreateAccount([FromBody] AccountRequest request)
        {
            try
            {
                var newAccount = new Account()
                {
                    AccountName = request.AccountName,
                    AccountType = request.AccountType,
                    OwnerUserId = request.OwnerUserId,
                };

                var createdAccount = _accountService.AddAccount(newAccount);

                var response = new AccountResponse()
                {
                    AccountName = createdAccount.AccountName,
                    Id = createdAccount.Id,
                    AccountType = createdAccount.AccountType,
                    Balance = createdAccount.Balance,
                };

                return Ok(response);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message); 
            }
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        public ActionResult<AccountResponse> UpdateAccountById([FromRoute] int id, [FromBody] UpdateAccountRequets updateRequest)
        {
            try
            {
                var updateAccount = new UpdateAccount()
                {
                    Id = id,
                    AccountName = updateRequest.AccountName,
                };

                var updatedAccount = _accountService.UpdateAccount(updateAccount);

                return Accepted(updatedAccount);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DontExistException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<AccountsController>/5
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteAccountById([FromRoute] int id)
        {
            try
            {
                _accountService.DeleteAccount(id);

                return NoContent();

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DontExistException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
