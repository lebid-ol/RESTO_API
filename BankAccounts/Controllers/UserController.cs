using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.ResponseModels;
using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Requests;
using BankAccounts.Shared.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using static BankAccounts.Shared.Models.GenderType;

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
        public ActionResult<List<UserResponse>> GetAllUsers()
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

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetUserById([FromRoute] int id)
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

        // POST api/<UserssController>
        [HttpPost]
        public ActionResult<UserResponse> CreateUser([FromBody] UserRequest request)
        {
           try
            {
                var newUser = new User()
                {
                    UserName = request.UserName,
                    Gender = request.Gender,
                    Email = request.Email,
                    UserLastName = request.UserLastName, 
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    BillingAddress = request.BillingAddress 
                };

                var createdUser = _userService.AddUser(newUser);

                var response = new UserResponse()
                {
                    UserName = createdUser.UserName,
                    Gender = createdUser.Gender,
                    Email = createdUser.Email,
                    UserLastName = createdUser.UserLastName,
                    PhoneNumber = createdUser.PhoneNumber,
                    DateOfBirth = createdUser.DateOfBirth,
                    BillingAddress = createdUser.BillingAddress
                };

                return Ok(response);
            }



            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateUserById([FromRoute] int userId, [FromBody] UpdateUserRequest updateRequest)
        {
            try
            {
                var updateUser = new UpdateUser()
                {
                    UserId = userId,
                    UserName = updateRequest.UserName,
                    Email = updateRequest.Email,
                    UserLastName = updateRequest.UserLastName,
                    PhoneNumber = updateRequest.PhoneNumber,
                    DateOfBirth = updateRequest.DateOfBirth,
                    BillingAddress = updateRequest.BillingAddress

                };

                var updatedUser = _userService.UpdateUser(updateUser);

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

