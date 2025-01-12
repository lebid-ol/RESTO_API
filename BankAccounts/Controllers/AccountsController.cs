using BankAccounts.Repositories;
using BankAccounts.RequestModel;
using BankAccounts.ResponseModels;
using BankAccounts.Services;
using Microsoft.AspNetCore.Mvc;


namespace BankAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // GET: api/<AccountsController>
        [HttpGet]
        public ActionResult<string> GetAllAccounts()
        {
            var allAccounts = AccountService.GetAccounts();

            if (allAccounts == null)
            {
                return NotFound(new { message = "Accounts not found in Database." });
            }

            return Ok(allAccounts);
        }

        [HttpGet("query/{id}")]
        public string GetAllAccountsQuery(
            [FromRoute] int id,
            [FromQuery] int[] aId)
        {
            if (aId.Length == 0)
            {
                return "Missing any aIDS";
            }

            return $"";

        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public ActionResult<string> GetAccountById([FromRoute] int id)
        {
            var account = AccountService.GetAccount(id);

            if (account == null)
            {
                return NotFound(new { message = "Account not found." });
            }

            return Ok(account);
        }

        // POST api/<AccountsController>
        [HttpPost]
        public ActionResult CreateAccount([FromBody] AccountRequest request)
        {
            AccountService.AddAccount(request);
            return Ok();
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        public string UpdateAccountById([FromRoute] int id, [FromBody] string name)
        {
            throw new Exception();
            return $"Account {id} updated to new name {name}";
        }

        // DELETE api/<AccountsController>/5
        [HttpDelete("{id}")]
        public string DeleteAccountById(int id)
        {
            return $"Account with ID {id} was deleted";
        }
    }
}
