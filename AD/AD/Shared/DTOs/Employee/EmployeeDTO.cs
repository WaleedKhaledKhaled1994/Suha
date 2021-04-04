using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Employee
{
    public class EmployeeDTO
    {
        public StoreUsers StoreUser { get; set; }
        public bool Admin { get; set; } = false;
        public bool ManageProducts { get; set; } = false;
        public bool ManageOrders { get; set; } = false;
        public bool ManageComments { get; set; } = false;
   }
}
