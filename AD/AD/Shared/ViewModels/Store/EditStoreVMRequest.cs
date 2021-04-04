using AD.Shared.DTOs.PaymentMethod;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Store
{
   public class EditStoreVMRequest
    {
        public long Id { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public long CityId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long CurrencyId { get; set; }
        public string Image { get; set; }
        public Status Status { get; set; }
        public List<long> Tags { get; set; }
        public List<long> Countries { get; set; }
        public List<long> Cities { get; set; }
        public List<PaymentMethodDTO> PaymentMethods { get; set; }
    }
}
