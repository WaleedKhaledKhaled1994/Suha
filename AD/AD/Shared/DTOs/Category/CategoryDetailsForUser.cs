using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Category
{
    public class CategoryDetailsForUser
    {
        public CategoryForUser Category { get; set; }
        public List<CategoryForUser> ChildCategories { get; set; } = new();
    }
}
