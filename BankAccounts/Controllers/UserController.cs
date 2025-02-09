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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<UserEntity>> GetAllUsers()
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
        public ActionResult<UserEntity> GetAllUsersQuery([FromQuery] string name)
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
        public ActionResult<UserEntity> GetUserById([FromRoute] int id)
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
        public ActionResult<UserEntity> CreateUser([FromBody] UserRequest request)
        {
            try
            {
                var user = _userService.AddUser(new Shared.Models.User());

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
                var updatedUser = _userService.UpdateUser(id, new Shared.Models.UserUpdate());

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
        public ActionResult<UserEntity> DeleteUserById([FromRoute] int id)
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

