using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Home
{
    public class HomePageVMResponse
    {
        public List<Entities.Tag> Tags { get; set; } = new();
        public List<Entities.Category> Categories { get; set; } = new();
        public List<Entities.Product> Products { get; set; } = new();
    }
}
