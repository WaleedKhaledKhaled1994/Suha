using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AD.Shared.DTOs.Auth
{
    public class LoginDTO
    {
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessageResourceName = "PhoneNumber_error", ErrorMessageResourceType = typeof(Resource))]
        //public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(20), MinLength(5)]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(18, ErrorMessageResourceName = "PasswordLength_error", ErrorMessageResourceType = typeof(Resource), MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessageResourceName = "Password_error", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
