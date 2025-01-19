using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.RequestModel;
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
        public ActionResult<List<Account>> GetAllAccounts()
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

        [HttpGet("query")]
        public ActionResult<Account> GetAllAccountsQuery([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
         
                return BadRequest("Missing name");

            }

            var account = AccountService.GetAccountByName(name);

            return Ok(account);

        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccountById([FromRoute] int id)
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
        public ActionResult<Account> CreateAccount([FromBody] AccountRequest request)
        {
            try
            {
                var account = AccountService.AddAccount(request);

                return Ok(account);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message); 
            }
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateAccountById([FromRoute] int id, [FromBody] UpdateAccountRequets updateRequest)
        {
            try
            {
                var updatedAccount = AccountService.UpdateAccount(id, updateRequest);

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
                AccountService.DeleteAccount(id);

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
