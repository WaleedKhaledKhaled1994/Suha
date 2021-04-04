using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Image
{
    public class AddImageVMRequest
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
