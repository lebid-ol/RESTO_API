using BankAccounts.Exceptions;
using BankAccounts.Models;
using BankAccounts.Repositories;
using BankAccounts.RequestModel;
using BankAccounts.ResponseModels;
using BankAccounts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                var allAccounts = AccountService.GetAccounts();

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
            try
            {
                var account = AccountService.GetAccount(id);

                return Ok(account);
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
        public ActionResult CreateAccount([FromBody] AccountRequest request)
        {
            AccountService.AddAccount(request);

            return Ok();
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateAccountById([FromRoute] int id, [FromBody] UpdateAccountRequets updateRequest)
        {
            AccountService.UpdateAccount(id, updateRequest);

            return Accepted();
        }

        // DELETE api/<AccountsController>/5
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteAccountById([FromRoute] int id)
        {
            try
            {
                var deleteOneAccount = AccountService.DeleteAccount(id);

                return Ok($"Account Id {deleteOneAccount.Id} with FirstName {deleteOneAccount.UserName} and LastName  {deleteOneAccount.UserLastName} was deleted");

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
