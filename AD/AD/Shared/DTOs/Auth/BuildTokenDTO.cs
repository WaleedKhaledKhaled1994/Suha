using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Auth
{
    public class BuildTokenDTO
    {
        //public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public long CurrencyId { get; set; }
        public string Password { get; set; }
    }
}
