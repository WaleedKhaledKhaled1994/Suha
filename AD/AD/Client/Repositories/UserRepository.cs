using AD.Client.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class UserRepository : IUserRepository
    {
        public string GetUserId()
        {
            throw new NotImplementedException();
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsSameUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
