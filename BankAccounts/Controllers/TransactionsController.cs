using BankAccounts.API.Requests;
using BankAccounts.API.Responses;
using BankAccounts.ApplicationService.Services;
using BankAccounts.AppplicationData.Db;
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
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly PostgresDbContext _context;
        private readonly IAccountService _accountService;


        public TransactionsController(
           IAccountService accountService,
           ITransactionService transactionService,
           IOptions<AzureSettingsOptions> azureOptions,
           PostgresDbContext context,
           IOptions<MyOptions> myOptions)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            var azureSettings = azureOptions.Value;
            var mySettings = myOptions.Value;
            _context = context;
        }

        // GET: api/transactions/account/{accountId}
        [HttpGet("account/{accountId:int}")]
        public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsByAccount(int accountId)
        {
            try
            {
                // (опционально) убеждаемся, что аккаунт существует
                var account = await _accountService.GetAccount(accountId);
                if (account is null) return NotFound($"Account {accountId} not found");

                var transactions = await _transactionService.GetTransactionsByAccount(accountId);

                var response = new List<TransactionResponse>();
                foreach (var t in transactions)
                {
                    response.Add(new TransactionResponse
                    {
                        Id = t.Id,
                        TransactionName = t.TransactionName,
                        Description = t.Description ?? string.Empty,
                        AmountTransaction = t.AmountTransaction,
                        Created = t.Created
                    });
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


        // GET api/<TransactionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TransactionsController>
        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> CreateTransaction([FromBody] TransactionRequest request)
        {
            try
            {
                var account = await _accountService.GetAccount(request.AccountId);
                if (account is null) return NotFound($"Account {request.AccountId} not found");

                var newTransaction = new Transaction()
                {
                    TransactionName = request.TransactionName,
                    Description= request.Description,
                    AmountTransaction = request.AmountTransaction,
                    AccountId = request.AccountId    // FK
    
                };


                var createdTransaction = await _transactionService.AddTransaction(newTransaction,  account, request.Type);

                var response = new TransactionResponse()
                {
                    TransactionName = createdTransaction.TransactionName,
                    Description = createdTransaction.Description,
                    AmountTransaction = createdTransaction.AmountTransaction,
                    Id = createdTransaction.Id,
                    Created = createdTransaction.Created
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
