using ActivityTracer.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracer.Data
{
	public class ActivityDbContext : DbContext
	{
		public DbSet<AppActivity> Activities { get; set; }

        public ActivityDbContext(DbContextOptions<ActivityDbContext> opt) : base(opt) { }

    }
}
