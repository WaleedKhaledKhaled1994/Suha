using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD.Shared.Entities;

namespace AD.Shared.DTOs.Category
{
    public class CategoryDTO
    {
        public Entities.Category Category { get; set; }
        public List<Entities.Category> UpperCategories { get; set; }
        public List<Entities.Category> ChildCategories { get; set; }
    }
}
