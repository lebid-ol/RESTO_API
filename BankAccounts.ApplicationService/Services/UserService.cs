using BankAccounts.AppplicationData.Records;
using BankAccounts.AppplicationData.Repositories;
using BankAccounts.Records;
using BankAccounts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccounts.ApplicationService.Services
{

    public class UserService
    {
        private readonly UsersRepository _usersRepository;

        public UserService()
        {
            _usersRepository = new UsersRepository();
        }

        public List<User> GetUsers()
        {
            var findAllUsers = _usersRepository.GetAllUsersFromData();

            return findAllUsers;

        }


    }
}
