using ActivityTracer.Data;
using ActivityTracer.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracer.Services
{
	public class FollowingService
	{
		private readonly ActivityDbContext _context;

		public FollowingService(ActivityDbContext context)
		{
			_context = context;
		}

		public async Task<bool> FollowUserAsync(string followerId, string followingId)
		{
			if (followerId == followingId)
				return false; // User cannot follow themselves

			var existingFollow = await _context.UserFollows
				.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

			if (existingFollow != null)
				return false; // Already following

			var follow = new UserFollow
			{
				FollowerId = followerId,
				FollowingId = followingId
			};
			
			_context.UserFollows.Add(follow);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> UnfollowUserAsync(string followerId, string followingId)
		{
			var existingFollow = await _context.UserFollows
				.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

			if (existingFollow == null)
				return false; // Not following

			_context.UserFollows.Remove(existingFollow);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> IsFollowingAsync(string followerId, string followingId)
		{
			return await _context.UserFollows
				.AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
		}

		public async Task<List<SiteUser>> GetFollowersAsync(string userId)
		{
			return await _context.UserFollows
				.Where(f => f.FollowingId == userId)
				.Select(f => f.Follower)
				.ToListAsync();
		}

		public async Task<List<SiteUser>> GetFollowingsAsync(string userId)
		{
			return await _context.UserFollows
				.Where(f => f.FollowerId == userId)
				.Select(f => f.Following)
				.ToListAsync();
		}
	}
}
