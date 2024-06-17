using ActivityTracer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActivityTracer.Controllers
{
	public class UsersController : Controller
	{
		private readonly FollowingService _followingService;

		public UsersController(FollowingService followingService)
		{
			_followingService = followingService;
		}

		[Authorize]
		public IActionResult OwnProfile()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Follow(string userId)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (await _followingService.FollowUserAsync(currentUserId, userId))
			{
				return RedirectToAction("Profile", "User", new { id = userId });
			}

			return BadRequest("Unable to follow user.");
		}

		[HttpPost]
		public async Task<IActionResult> Unfollow(string userId)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (await _followingService.UnfollowUserAsync(currentUserId, userId))
			{
				return RedirectToAction("Profile", "User", new { id = userId });
			}

			return BadRequest("Unable to unfollow user.");
		}

		[HttpGet]
		public async Task<IActionResult> Followers(string userId)
		{
			var followers = await _followingService.GetFollowersAsync(userId);
			return View(followers);
		}

		[HttpGet]
		public async Task<IActionResult> Followings(string userId)
		{
			var followings = await _followingService.GetFollowingsAsync(userId);
			return View(followings);
		}
	}
}
