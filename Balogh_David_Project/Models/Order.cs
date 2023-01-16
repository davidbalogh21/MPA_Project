using System;
namespace Balogh_David_Project.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int LaptopID { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer? Customer { get; set; }
        public Laptop? Laptop { get; set; }
    }
}

