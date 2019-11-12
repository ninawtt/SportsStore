using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        // Constructor
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) 
            : base(options) { } // calling DbContext's constructor (Parent's constructor)
    }
}
