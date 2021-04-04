using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IUserRepository
    {
        string GetUserId();

        bool IsInRole(string role);

        bool IsSameUser(string userId);
    }
}
