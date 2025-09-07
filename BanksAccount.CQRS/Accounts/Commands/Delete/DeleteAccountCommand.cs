using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BanksAccount.CQRS.Accounts.Commands.Delete
{
    public class DeleteAccountCommand : IRequest
    {
        public int AccountId { get;set; }

        public DeleteAccountCommand(int accountId)
        {
            AccountId = accountId;
        }
    }
}
