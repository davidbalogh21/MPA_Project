using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Balogh_David_Project.Models
{
    public class Distributor
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Distributor Name")]
        [StringLength(50)]
        public string DistributorName { get; set; }

        [StringLength(70)]
        public string Address { get; set; }
        public ICollection<DistributedLaptop>? DistributedLaptops { get; set; }
    }
}

