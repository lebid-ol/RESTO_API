using BankAccounts.ApplicationService.Services;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.RequestModel;
using BankAccounts.Services;
using BankAccounts.Shared.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
          private readonly UserService _userService;

          public UserController()
          {
             _userService = new UserService();
          }

         //GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            try
            {
                var userService = new UserService();
                var allUsers = _userService.GetUsers();

                return Ok(allUsers);

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
            var accountService = new AccountService();
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

    }
}
