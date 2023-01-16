using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;

namespace Balogh_David_Project.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Column(TypeName = "text")]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Address { get; set; }

        public DateTime BirthDate { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}

