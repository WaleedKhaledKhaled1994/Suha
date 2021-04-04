using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Category
{
    public class CategoryForUser
    {
        public Entities.Category Category { get; set; }
        public FollowStatus Status { get; set; }
    }
}
