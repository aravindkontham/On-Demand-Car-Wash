using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.DTO
{
    public class PackageDto
    {
         public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}