using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Product
{
    public class ProductDetailsDTO
    {
        public Entities.Product Product { get; set; } = new Entities.Product();
        public List<string> Colors { get; set; } = new List<string>();
        public List<string> Images { get; set; } = new List<string>();
        public List<string> OldColors { get; set; } = new List<string>();
        public List<string> OldImages { get; set; } = new List<string>();
        public List<Entities.Category> UpperCategories { get; set; }
    }
}
