using Microsoft.AspNetCore.Identity;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Models
{
    public class ApplicationUser : IdentityUser, IBaseEntity
    {
        long IBaseEntity.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
