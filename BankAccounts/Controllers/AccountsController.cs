using BankAccounts.API.RequestValidators;
using BanksAccount.CQRS.Accounts.Commands.Update;
using BankAccounts.ApplicationService.Services;
using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.RequestModel;
using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Accounts.Commands.Delete;
using BanksAccount.CQRS.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ISender _sender;
        private readonly AccountRequestValidator _accountRequestValidator;

        public AccountsController(
            IAccountService accountService,
            ITransactionService transactionService,
            IOptions<AzureSettingsOptions> azureOptions,
            IOptions<MyOptions> myOptions,
            ISender sender)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            var azureSettings = azureOptions.Value;
            var mySettings = myOptions.Value;
            _sender = sender;
            _accountRequestValidator = new AccountRequestValidator();
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
                        Balance = account.Balance,
                        BalanceEuro = account.BalanceInEuro
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

        // GET api/account/{accountId}?from=2025-08-01&to=2025-08-16
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<AccountResponse>> GetAccountById(
            [FromRoute] int id,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        { 
            {
                try
                {
                    var query = new GetAccountByIdQuery(id);
                    
                    var response = await _sender.Send(query);

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
        }

        // POST api/<AccountsController>
        [HttpPost]
        public async Task<ActionResult<AccountResponse>> CreateAccount([FromBody] AccountRequest request)
        {
            try
            {
                var result = await _accountRequestValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                
                var createCommand = new CreateAccountCommand(
                   request.AccountName,
                   request.AccountType,
                   request.UserId);

                return await _sender.Send(createCommand);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message); 
            }
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountResponse>> UpdateAccountById([FromRoute] int id, [FromBody] UpdateAccountRequets updateRequest)
        {
            try
            {
                var cmd = new UpdateAccountCommand(id, updateRequest.AccountName);
                var response = await _sender.Send(cmd);

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

        // DELETE api/<AccountsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteAccountById([FromRoute] int id)
        {
            try
            {
                var deleteCommand = new DeleteAccountCommand(id);   

                await _sender.Send(deleteCommand);  

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
