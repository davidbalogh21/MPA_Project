using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Balogh_David_Project.Areas.Identity.Data;

public class Balogh_David_ProjectIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public Balogh_David_ProjectIdentityDbContext(DbContextOptions<Balogh_David_ProjectIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
