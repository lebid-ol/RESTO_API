using BankAccounts.AppplicationData.Records;
using BankAccounts.AppplicationData.Repositories;
using BankAccounts.Records;
using BankAccounts.Shared.Models;

namespace BankAccounts.Services
{
    public interface IUserService
    {
        UserEntity AddUser(User userRequest);
        UserEntity? GetUser(int id);
        List<User> GetUsers();
        UserEntity UpdateUser(int id, UserUpdate requets);
        void DeleteUser(int id);
        AccountEntity GetUserByName(string name);

    }

    public class UserService : IUserService
    {
        private readonly IUserRepositoy _usersRepository;

        public UserService(IUserRepositoy usersRepository)
        {
            _usersRepository = usersRepository;

        }

        public UserEntity AddUser(User userRequest)
        {

            var user = new UserEntity()
            {
                //Email = userRequest.Email,
                //UserName = userRequest.UserName,
                //Id = GetNextUserID(),
                //UserLastName = userRequest.UserLastName,
                //PhoneNumber = userRequest.PhoneNumber,
                //DateOfBirth = userRequest.DateOfBirth,
                //Gender = userRequest.Gender,
                //BillingAddress = userRequest.BillingAddress
            };

            var users = new List<UserEntity>()
            {
                user
            };

            _usersRepository.AddUserRecord(users);

            return user;
        }

        private int GetNextUserID()
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

        public UserEntity? GetUser(int id)
        {

            var findUser = _usersRepository.GetOneUserFromData(id);

            if (findUser == null)
            {
                return null;
            }

            return findUser;

        }

        public List<UserEntity> GetUsers()
        {
            var findAllUser = _usersRepository.GetAllUsersFromData();

            return findAllUser;

        }

        public UserEntity UpdateUser(int id, UserUpdate requests)
        {
            var updatedUser = new UserEntity()
            {
                //Id = id,
                //Email = requests.Email,
                //UserName = requests.UserName,
                //UserLastName = requests.UserLastName,
                //PhoneNumber = requests.PhoneNumber,
                //DateOfBirth = requests.DateOfBirth,
                //Gender = requests.Gender,
                //BillingAddress = requests.BillingAddress

            };


            return _usersRepository.UpdateUserRecord(updatedUser);
        }

        public void DeleteUser(int id)
        {
            _usersRepository.DeleteUserFromData(id);
        }

        public UserEntity GetUserByName(string name)
        {
            return _usersRepository.GetUserByName(name);
        }

        List<User> IUserService.GetUsers()
        {
            throw new NotImplementedException();
        }

        AccountEntity IUserService.GetUserByName(string name)
        {
            throw new NotImplementedException();
        }


       

      
    }


}
