using BankAccounts.AppplicationData.Records;
using BankAccounts.AppplicationData.Repositories;
using BankAccounts.Records;
using BankAccounts.Repositories;
using BankAccounts.Shared.Models;

namespace BankAccounts.Services
{
    public interface IUserService
    {
        User AddUser(User userRequest);
        Task <User> GetUser(string id);
        Task <List <User>> GetUsers();
        Task <User> UpdateUser(UpdateUser requets);
        Task DeleteUser(string id);

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;
        private readonly IAccountRepository _accountsRepository;

        public UserService(IUserRepository usersRepository, IAccountRepository accountsRepository)
        {
            _usersRepository = usersRepository;
            _accountsRepository = accountsRepository;

        }

        public User AddUser(User user)
        {
            var createdUser = _usersRepository.AddUserRecord(user);

             return createdUser;
        }

        public int GetNextUserID()
        {
            if (!File.Exists("userId.txt"))
            {
                File.WriteAllText("userId.txt", "1");

                return 1;
            }
            else
            {
                var id = File.ReadAllText("userId.txt");
                var intId = int.Parse(id);
                var nextId = intId + 1;
                File.WriteAllText("userId.txt", nextId.ToString());
                return nextId;
            }

        }

        public async Task <User> GetUser(string id)
        {

            var user = await _usersRepository.GetOneUserFromData(id);
            var accounts = await _accountsRepository.GetAllAccountsByOwnerId(id);

            user.Accounts = accounts;

            return user;
           
        }

        public Task <List<User>> GetUsers()
        {
            var findAllUser = _usersRepository.GetAllUsersFromData();

            return findAllUser;

        }

         public Task <User> UpdateUser(UpdateUser updatedUser)
        {
            var user = _usersRepository.UpdateUserRecord(updatedUser);
            return user;
        }

        public Task DeleteUser(string id)
        {
           return  _usersRepository.DeleteUserFromData(id);
        }

    }


}
