using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Balogh_David_Project.Models
{
	public class Laptop
	{
        public int ID { get; set; }

        [Column(TypeName = "text")]
        public string Name { get; set; }


        public Manufacturer? Manufacturer { get; set; }
        public int ManufacturerID { get; set; }

        public int Price { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<DistributedLaptop>? DistributedLaptops { get; set; }
    }
}

