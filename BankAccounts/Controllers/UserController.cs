using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.Services;
using BankAccounts.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            try
            {
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
        public ActionResult<User> GetAllUsersQuery([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {

                return BadRequest("Missing name");

            }
               var user = _userService.GetUserByName(name);

            return Ok(user);

        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = _userService.GetUser(id);

                return Ok(user);
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
        public ActionResult<User> CreateUser([FromBody] UserRequest request)
        {
            try
            {
                var user = _userService.AddUser(request);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateUserById([FromRoute] int id, [FromBody] UpdateUserRequest updateRequest)
        {
            try
            {
                var updatedUser = _userService.UpdateUser(id, updateRequest);

                return Accepted(updatedUser);
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

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUserById([FromRoute] int id)
        {
            try
            {
                _userService.DeleteUser(id);

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

