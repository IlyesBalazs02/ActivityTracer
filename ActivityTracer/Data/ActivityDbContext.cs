using ActivityTracer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracer.Data
{
	public class ActivityDbContext : IdentityDbContext
	{
		public DbSet<AppActivity> Activities { get; set; }
		public DbSet<SiteUser> Users { get; set; }

        public ActivityDbContext(DbContextOptions<ActivityDbContext> opt) : base(opt) { }

    }
}
