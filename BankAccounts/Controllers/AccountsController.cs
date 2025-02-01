using BankAccounts.API.DI_test;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.RequestModel;
using BankAccounts.Services;
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
        private readonly AccountService _accountService;


        public AccountsController(AccountService accountService)
        {
             _accountService = accountService;
           
        }

        // GET: api/<AccountsController>
        [HttpGet]
        public ActionResult<List<Account>> GetAllAccounts()
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

        [HttpGet("query")]
        public ActionResult<Account> GetAllAccountsQuery([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
         
                return BadRequest("Missing name");

            }
            var account = _accountService.GetAccountByName(name);

            return Ok(account);

        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccountById([FromRoute] int id)
        {
            try
            {

                var account = _accountService.GetAccount(id);

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
                var account = _accountService.AddAccount(request);

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
                var updatedAccount = _accountService.UpdateAccount(id, updateRequest);

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
