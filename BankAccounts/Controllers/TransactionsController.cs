using BankAccounts.API.Requests;
using BankAccounts.API.Responses;
using BanksAccount.CQRS.Transactions.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Exceptions;
using BankAccounts.Shared.Exceptions;
using BanksAccount.CQRS.Transactions.Queries;



namespace BankAccounts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ISender _sender;
        public TransactionsController(ISender sender) => _sender = sender;

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> CreateTransaction([FromBody] TransactionRequest request)
        {
            try
            {
                if (request is null) return BadRequest("Body is required.");

                var cmd = new CreateTransactionCommand(
                    accountId: request.AccountId,
                    transactionName: request.TransactionName,
                    description: request.Description,
                    amountTransaction: request.AmountTransaction,
                    type: request.Type
                );

                var response = await _sender.Send(cmd);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/transactions/{accountId}?from=2025-08-01&to=2025-08-16
        [HttpGet("{accountId:int}")]
        public async Task<ActionResult<List<TransactionResponse>>> GetByAccount(
            [FromRoute] int accountId,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            try
            {
                var query = new GetTransactionsByQuery(accountId, from, to);
                var response = await _sender.Send(query);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

