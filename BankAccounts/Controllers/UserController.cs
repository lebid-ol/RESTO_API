using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.ResponseModels;
using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Requests;
using BankAccounts.Shared.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            try
            {
                var allUsers = await _userService.GetUsers();
                var response = new List<UserResponse>();
                foreach (var user in allUsers)
                {
                    var userResponse = new UserResponse()
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        UserLastName = user.UserLastName,
                        PhoneNumber = user.PhoneNumber,
                        DateOfBirth = user.DateOfBirth,
                        Gender = user.Gender,
                        BillingAddress = user.BillingAddress
                    };                    ;

                    response.Add(userResponse);
                }

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
        public async Task <ActionResult<UserWithAccounts>> GetUserById([FromRoute] string id)
        {
            try
            {
                var userWithAccounts = await _userService.GetUser(id);
               
               /* var response = new UserResponse()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    UserLastName = user.UserLastName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    BillingAddress = user.BillingAddress,
                                        
                 };*/

                return Ok(userWithAccounts);
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
        public async Task <ActionResult<UserResponse>> CreateUser([FromBody] UserRequest request)
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
        public async Task <ActionResult<UserResponse>>UpdateUserById([FromRoute] string userId, [FromBody] UpdateUserRequest updateRequest)
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

                var updatedUser = await _userService.UpdateUser(updateUser);

                var response = new UserResponse()
                {
                    UserName = updateUser.UserName,
                    Email = updateUser.Email,
                    UserLastName = updateUser.UserLastName,
                    PhoneNumber = updateUser.PhoneNumber,
                    DateOfBirth = updateUser.DateOfBirth,
                    BillingAddress = updateUser.BillingAddress
                };

                return Accepted(response);
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
        public async Task <ActionResult<string>> DeleteUserById([FromRoute] string id)
        {
            try
            {
               await  _userService.DeleteUser(id);

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

