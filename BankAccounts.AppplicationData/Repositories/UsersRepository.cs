using BankAccounts.Exceptions;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Shared.Models;
using BankAccounts.AppplicationData.Db;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.AppplicationData.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUserRecord(User users);
        Task <User> GetOneUserFromData(int userId);
        Task<List<User>> GetAllUsersFromData();
        Task <User> UpdateUserRecord(UpdateUser user);
        Task DeleteUserFromData(int userId);
       
        
    }

    public class UsersRepository : IUserRepository
    {
        private readonly PostgresDbContext _postgresContext;

        public UsersRepository(PostgresDbContext postgresContext)
        {
            _postgresContext = postgresContext;
        }

        public async Task<User> AddUserRecord(User user)
        {
            var userEntity = new UserEntity
            {
                UserName = user.UserName,
                Email = user.Email,
                UserLastName = user.UserLastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth.ToUniversalTime(),
                BillingAddress = user.BillingAddress,
            };

            var newUserEntity = await _postgresContext.Users.AddAsync(userEntity);
            await _postgresContext.SaveChangesAsync();

            user.UserId = userEntity.Id;

            return user;
        }


        public async Task <User> GetOneUserFromData(int userId)
        {
            var userEntity = await _postgresContext.Users.FindAsync(userId);

            if (userEntity != null)
            {
                var user = new User()
                {
                    UserName = userEntity.UserName,
                    Email = userEntity.Email,
                    UserLastName = userEntity.UserLastName,
                    PhoneNumber = userEntity.PhoneNumber,
                    DateOfBirth = userEntity.DateOfBirth,
                    BillingAddress = userEntity.BillingAddress,

                };

                return user;
            }

            throw new NotFoundException("No users records found");
        }


        public async Task<List<User>> GetAllUsersFromData()
        {
            var users = await _postgresContext.Users.ToListAsync();

            var userList = new List<User>();

            foreach (var record in users)
            {
                var user = new User()
                {
                    UserId = record.Id,
                    UserName = record.UserName,
                    Email = record.Email,
                    UserLastName = record.UserLastName,
                    PhoneNumber = record.PhoneNumber,
                    DateOfBirth = record.DateOfBirth,
                    BillingAddress = record.BillingAddress,
                    Gender = record.Gender

                };

                userList.Add(user);

            }
            return userList;
        }
    

        public async Task <User> UpdateUserRecord(UpdateUser user)
        {
            var userToUpdate = await _postgresContext.Users.FindAsync(user.UserId);

            if (userToUpdate == null)
            {
                throw new NotFoundException("No users records found");
            }

            userToUpdate.UserName = user.UserName;
            userToUpdate.Email = user.Email;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.UserLastName = user.UserLastName;
            userToUpdate.BillingAddress = user.BillingAddress;
            userToUpdate.DateOfBirth = user.DateOfBirth.ToUniversalTime();

           await _postgresContext.SaveChangesAsync();

            return new User
            {
                UserId = userToUpdate.Id,
                DateOfBirth = userToUpdate.DateOfBirth,
                BillingAddress = userToUpdate.BillingAddress,
                UserLastName = userToUpdate.UserLastName,
                PhoneNumber = userToUpdate.PhoneNumber,
                Email = userToUpdate.Email,
                UserName = userToUpdate.UserName,
            };
        }

        public async Task DeleteUserFromData(int userId)
        {
            var userToDelete = await _postgresContext.Users.FindAsync(userId);

            if (userToDelete == null)
            {
                throw new NotFoundException("No users records found");
            }

            _postgresContext.Users.Remove(userToDelete);

            await _postgresContext.SaveChangesAsync();
        }

    }
}



