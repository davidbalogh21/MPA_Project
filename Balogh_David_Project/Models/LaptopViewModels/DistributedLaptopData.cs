using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Balogh_David_Project.Models.LaptopViewModels
{
	public class DistributedLaptopData
	{
        public int LaptopID { get; set; }

        [Column(TypeName = "text")]
        public string Name { get; set; }
        public bool IsPublished { get; set; }
    }
}

