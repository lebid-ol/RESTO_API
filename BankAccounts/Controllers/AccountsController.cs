using BankAccounts.AppplicationData.DbContext;
using BankAccounts.Exceptions;
using BankAccounts.RequestModel;
using BankAccounts.ResponseModels;
using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(
            IAccountService accountService,
            IOptions<AzureSettingsOptions> azureOptions,
            IOptions<MyOptions> myOptions)
        {
            _accountService = accountService;
            var azureSettings = azureOptions.Value;
            var mySettings = myOptions.Value;
            Console.WriteLine();
        }

        // GET: api/<AccountsController>
        [HttpGet]
        public async Task<ActionResult<List<AccountResponse>>> GetAllAccounts()
        {

            var requsetHeaders = Request.Headers;
            try
            {
                var allAccounts = await _accountService.GetAccounts();
                var response =  new List<AccountResponse>();
                foreach (var account in allAccounts)
                {
                    var accountResponse = new AccountResponse
                    {
                        Id = account.Id,
                        AccountName = account.AccountName,
                        AccountType = account.AccountType,
                        Balance = account.Balance
                    };

                    response.Add(accountResponse);
                }

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
        public async Task<ActionResult<AccountResponse>> CreateAccount([FromBody] AccountRequest request)
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
        public async Task<ActionResult<AccountResponse>> UpdateAccountById([FromRoute] string id, [FromBody] UpdateAccountRequets updateRequest)
        {
            try
            {
                var updateAccount = new UpdateAccount()
                {
                    Id = id,
                    AccountName = updateRequest.AccountName,
                };

                var updatedAccount = await _accountService.UpdateAccount(updateAccount);

                var response = new AccountResponse()
                {
                    AccountName = updatedAccount.AccountName,
                    Id = updatedAccount.Id,
                    AccountType = updatedAccount.AccountType,
                    Balance = updatedAccount.Balance
                };

                return Accepted(response);
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
        public async Task<ActionResult<string>> DeleteAccountById([FromRoute] string id)
        {
            try
            {
                await _accountService.DeleteAccount(id);

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
