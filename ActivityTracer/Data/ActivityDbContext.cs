using ActivityTracer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracer.Data
{
	public class ActivityDbContext : IdentityDbContext
	{
		public DbSet<AppActivity> Activities { get; set; }
		public DbSet<SiteUser> Users { get; set; }
		public DbSet<UserFollow> UserFollows { get; set; }

        public ActivityDbContext(DbContextOptions<ActivityDbContext> opt) : base(opt) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<AppActivity>()
				.HasOne(t => t.Owner)
				.WithMany()
				.HasForeignKey(t => t.OwnerId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<UserFollow>()
			   .HasKey(uf => new { uf.FollowerId, uf.FollowingId });

			builder.Entity<UserFollow>()
				.HasOne(uf => uf.Follower)
				.WithMany(u => u.Followings)
				.HasForeignKey(uf => uf.FollowerId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<UserFollow>()
				.HasOne(uf => uf.Following)
				.WithMany(u => u.Followers)
				.HasForeignKey(uf => uf.FollowingId)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(builder);
		}
	}
}
