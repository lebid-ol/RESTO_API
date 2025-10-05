using BankAccounts.API.RequestValidators;
using BankAccounts.ApplicationService.Services;
using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Services;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Requests;
using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Accounts.Commands.Delete;
using BanksAccount.CQRS.Accounts.Queries;
using BanksAccount.CQRS.Users.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankAccounts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISender _sender;

        public UserController(IUserService userService, ISender sender)
        {
            _userService = userService;
            _sender = sender;
        }

        //GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            var requsetHeaders = Request.Headers;

            try
            {
                var allUsers = await _userService.GetUsers();
                var response = new List<UserResponse>();
                foreach (var user in allUsers)
                {
                    var userResponse = new UserResponse()
                    {
                        Id = user.UserId,
                        UserName = user.UserName,
                        Email = user.Email,
                        UserLastName = user.UserLastName,
                        PhoneNumber = user.PhoneNumber,
                        DateOfBirth = user.DateOfBirth,
                        Gender = user.Gender,
                        BillingAddress = user.BillingAddress
                       
                    };                    

                    response.Add(userResponse);
                }

                return Ok(response);

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
        public async Task <ActionResult<UserResponse>> GetUserById([FromRoute] int id)
        {

            try
            {
                var query = new GetUserByIdQuery(id);

                var response = await _sender.Send(query);

                return Ok(response);

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

        // POST api/<UsersController>
        [HttpPost]
        public async Task <ActionResult<UserResponse>> CreateUser([FromBody] UserRequest request)
        {
            try
            {
                var createCommand = new CreateUserCommand(
                   request.UserName,
                   request.Email,
                   request.UserLastName,
                   request.PhoneNumber,
                   request.DateOfBirth,
                   request.Gender,
                   request.BillingAddress);
 
                return await _sender.Send(createCommand);
            }          

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task <ActionResult<UserResponse>>UpdateUserById([FromRoute] int id, [FromBody] UpdateUserRequest updateRequest)
        {
            try
            {
                var updateUser = new UpdateUser()
                {
                    UserId = id,
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
                    Id = updatedUser.UserId,
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
        public async Task <ActionResult<string>> DeleteUserById([FromRoute] int id)
        {
            try
            {
                var deleteCommand = new DeleteUserCommand(id);

                await _sender.Send(deleteCommand);

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

