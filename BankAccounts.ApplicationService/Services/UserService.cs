using BankAccounts.AppplicationData.Records;
using BankAccounts.AppplicationData.Repositories;
using BankAccounts.Records;
using BankAccounts.Shared.Models;

namespace BankAccounts.Services
{
    public interface IUserService
    {
        User AddUser(User userRequest);
        User? GetUser(int id);
        IEnumerable<User> GetUsers();
        User UpdateUser(UpdateUser requets);
        void DeleteUser(int id);

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;

        public UserService(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;

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

        public User? GetUser(int id)
        {

            var findUser = _usersRepository.GetOneUserFromData(id);

            if (findUser == null)
            {
                return null;
            }

            return findUser;

        }

        public Task <List<User>> GetUsers()
        {
            var findAllUser = _usersRepository.GetAllUsersFromData();

            return findAllUser;

        }

         public User UpdateUser(UpdateUser updatedUser)
        {
           // var updatedUser = new UserEntity()
            //{
                //Id = id,
                //Email = requests.Email,
                //UserName = requests.UserName,
                //UserLastName = requests.UserLastName,
                //PhoneNumber = requests.PhoneNumber,
                //DateOfBirth = requests.DateOfBirth,
                //Gender = requests.Gender,
                //BillingAddress = requests.BillingAddress

           // };

            var user = _usersRepository.UpdateUserRecord(updatedUser);
            return user;
        }

        public void DeleteUser(int id)
        {
            _usersRepository.DeleteUserFromData(id);
        }

        IEnumerable<User> IUserService.GetUsers()
        {
            throw new NotImplementedException();
        }
    }


}
