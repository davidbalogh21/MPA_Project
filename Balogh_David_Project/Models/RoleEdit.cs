using System;
using Microsoft.AspNetCore.Identity;

namespace Balogh_David_Project.Models
{
	public class RoleEdit
	{
        public IdentityRole? Role { get; set; }
        public IEnumerable<IdentityUser>? Members { get; set; }
        public IEnumerable<IdentityUser>? NonMembers { get; set; }
    }
}

