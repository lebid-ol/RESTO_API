using BankAccounts.API.Requests;
using BankAccounts.API.Responses;
using BankAccounts.ApplicationService.Services;
using BankAccounts.AppplicationData.Db;

using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;


namespace BankAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        //private readonly PostgresDbContext _context;
        private readonly IAccountService _accountService;


        public TransactionsController(
           IAccountService accountService,
           ITransactionService transactionService,
           IOptions<AzureSettingsOptions> azureOptions,
           //PostgresDbContext context,
           IOptions<MyOptions> myOptions)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            var azureSettings = azureOptions.Value;
            var mySettings = myOptions.Value;
            //_context = context;
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
