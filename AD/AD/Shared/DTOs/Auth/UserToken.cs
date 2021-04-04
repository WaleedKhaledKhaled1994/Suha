using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Shared.DTOs.Auth
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
