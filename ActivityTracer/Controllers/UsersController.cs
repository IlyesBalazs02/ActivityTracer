using ActivityTracer.Models;
using ActivityTracer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActivityTracer.Controllers
{
	public class UsersController : Controller
	{
		private readonly FollowingService _followingService;
		private readonly UserManager<SiteUser> _userManager;

		public UsersController(FollowingService followingService, UserManager<SiteUser> userManager)
		{
			_followingService = followingService;
			_userManager = userManager;
		}

		[Authorize]
		public async Task<IActionResult> OwnProfile()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		[Authorize]
		public async Task<IActionResult> Profile(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		[Authorize]
		public IActionResult ListUsers()
		{
			return View(_userManager.Users.Where( t => t.Id != _userManager.GetUserId(this.User)));
		}

		public async Task<IActionResult> Follow(string userId)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			
			if (await _followingService.FollowUserAsync(currentUserId, userId))
			{
				return RedirectToAction(nameof(ListUsers));
			}

			return BadRequest("Unable to follow user.");
		}

		public async Task<IActionResult> Unfollow(string userId)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (await _followingService.UnfollowUserAsync(currentUserId, userId))
			{
				return RedirectToAction(nameof(ListUsers));
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
