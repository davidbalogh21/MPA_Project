using System;
namespace Balogh_David_Project.Models
{
    public class DistributedLaptop
    {
        public int? DistributorID { get; set; }
        public int LaptopID { get; set; }
        public Distributor? Distributor { get; set; }
        public Laptop? Laptop { get; set; }
    }
}

