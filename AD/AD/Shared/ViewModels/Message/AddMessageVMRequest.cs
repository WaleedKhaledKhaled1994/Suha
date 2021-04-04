using AD.Shared.Entities.Base.Enums;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Message
{
    public class AddMessageVMRequest
    {
        public long OrderId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(200, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string Text { get; set; }
        public MessageType MessageType { get; set; }
    }
}
