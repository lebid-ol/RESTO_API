using BankAccounts.AppplicationData.Records;
using BankAccounts.AppplicationData.Repositories;
using BankAccounts.Records;
using BankAccounts.Shared.Models.Requests;

namespace BankAccounts.Services
{
    public interface IUserService
    {
        User AddUser(UserRequest userRequest);
        User? GetUser(int id);
        List<UserRequest> GetUsers();
        User UpdateUser(int id, UpdateUserRequest requets);
        void DeleteUser(int id);
        Account GetUserByName(string name);

    }

    public class UserService : IUserService
    {
        private readonly UsersRepository _usersRepository;

        public UserService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;

        }

        public User AddUser(UserRequest userRequest)
        {

            var user = new User()
            {
                Email = userRequest.Email,
                UserName = userRequest.UserName,
                Id = GetNextUserID(),
                UserLastName = userRequest.UserLastName,
                PhoneNumber = userRequest.PhoneNumber,
                DateOfBirth = userRequest.DateOfBirth,
                Gender = userRequest.Gender,
                BillingAddress = userRequest.BillingAddress
            };

            var users = new List<User>()
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

        public User? GetUser(int id)
        {

            var findUser = _usersRepository.GetOneUserFromData(id);

            if (findUser == null)
            {
                return null;
            }

            return findUser;

        }

        public List<User> GetUsers()
        {
            var findAllUser = _usersRepository.GetAllUsersFromData();

            return findAllUser;

        }

        public User UpdateUser(int id, UpdateUserRequest requests)
        {
            var updatedUser = new User()
            {
                Id = id,
                Email = requests.Email,
                UserName = requests.UserName,
                UserLastName = requests.UserLastName,
                PhoneNumber = requests.PhoneNumber,
                DateOfBirth = requests.DateOfBirth,
                Gender = requests.Gender,
                BillingAddress = requests.BillingAddress

            };


            return _usersRepository.UpdateUserRecord(updatedUser);
        }

        public void DeleteUser(int id)
        {
            _usersRepository.DeleteUserFromData(id);
        }

        public User GetUserByName(string name)
        {
            return _usersRepository.GetUserByName(name);
        }

        List<UserRequest> IUserService.GetUsers()
        {
            throw new NotImplementedException();
        }

        Account IUserService.GetUserByName(string name)
        {
            throw new NotImplementedException();
        }
    }


}
