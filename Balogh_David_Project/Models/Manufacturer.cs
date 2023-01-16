using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Balogh_David_Project.Models
{
    public class Manufacturer
    {
        public int ID { get; set; }

        [Column(TypeName = "text")]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Location { get; set; }

        public ICollection<Laptop>? Laptops { get; set; }
    }
}

