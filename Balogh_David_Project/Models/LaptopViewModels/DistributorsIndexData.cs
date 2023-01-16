using System;
using System.Security.Policy;

namespace Balogh_David_Project.Models.LaptopViewModels
{
	public class DistributorsIndexData
	{
        public IEnumerable<Distributor> Distributors{ get; set; }
        public IEnumerable<Laptop> Laptops { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}

