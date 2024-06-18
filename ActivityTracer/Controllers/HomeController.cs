using ActivityTracer.Data;
using ActivityTracer.Migrations;
using ActivityTracer.Models;
using ActivityTracer.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ActivityTracer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly FollowingService _followingService;
        IAppActivityRepository repository;

        BlobServiceClient serviceClient;
        BlobContainerClient containerClient;

		public HomeController(ILogger<HomeController> logger, UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManager, IAppActivityRepository repository, FollowingService followingService)
		{
			_logger = logger;
			_userManager = userManager;
			_roleManager = roleManager;
			this.repository = repository;
            this._followingService = followingService;
            serviceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=activitytracerstorage;AccountKey=N/7GzEAl1Y4wxc/YQLMAt0wae1h6o25vbjniMn3jL8zim7B5McogkoLJ1AgXJnKrHAEe3ieeM0O8+AStF40ECw==;EndpointSuffix=core.windows.net");
            containerClient = serviceClient.GetBlobContainerClient("photos");
		}

		public IActionResult Index()
        {
            return View();
        }

		[Authorize]
		public async Task<IActionResult> MainPage()
        {
            var thisUserId = _userManager.GetUserId(this.User);
            var followings = await _followingService.GetFollowingsAsync(thisUserId);
            followings.Add(await _userManager.GetUserAsync(User));

			//Select this user's activities and those activities that belong to the users it follows.
			var activities = repository.Read().Where(t => followings.Select(x => x.Id).Contains(t.OwnerId));
            return View(activities.OrderByDescending(t => t.Date));
        }

		[Authorize]
		public IActionResult Create()
        {
            var activity = new AppActivity();
            return View(activity);
        }

		[Authorize]
		[HttpPost]
        public async Task<IActionResult> Create(AppActivity appActivity, [FromForm] List<IFormFile> photoUpload)
        {
            appActivity.OwnerId = _userManager.GetUserId(this.User);
			appActivity.PhotoUrl = new List<string>();
            


			string formattedDate = appActivity.Date.ToString("yyyyMMdd_HHmmss");

            int i = 0;
            foreach (var photo in photoUpload)
            {

                if (photo.Length > 0)
                {
                    ++i;

                    BlobClient blobClient = containerClient.GetBlobClient(appActivity.OwnerId + "_" + formattedDate + appActivity.Title.Replace(" ", "").ToLower() + i);
                    using (var uploadFileStream = photo.OpenReadStream())
                    {
                        await blobClient.UploadAsync(uploadFileStream, true);
                    }
                    blobClient.SetAccessTier(AccessTier.Cool);

                    appActivity.PhotoUrl.Add(blobClient.Uri.AbsoluteUri);
                }
            }
			// Ignore Owner and OwnerId from ModelState.
			ModelState.Remove("Owner");
            ModelState.Remove("OwnerId");
            if (!ModelState.IsValid)
            {
                return View(appActivity);

            }
            
            repository.Create(appActivity);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult List()
        {
            return View(this.repository.Read());
        }

        public IActionResult Edit(string id)
        {
            var appActivity = repository.ReadFromId(id);
            return View(appActivity);
        }

        [HttpPost]
        public IActionResult Edit(AppActivity appActivity)
        {
			appActivity.OwnerId = _userManager.GetUserId(this.User);

			// Ignore Owner and OwnerId from ModelState.
			ModelState.Remove("Owner");
			ModelState.Remove("OwnerId");
			if (!ModelState.IsValid)
			{
				return View(appActivity);

			}
			repository.Update(appActivity);
            return RedirectToAction(nameof(Index));
        }

        //get user profile picture
        public IActionResult GetImage(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == userId);
            return new FileContentResult(user.Data, user.ContentType);
        }

        public async Task<IActionResult> DelegateAdmin()
        {
			var principal = this.User;
			var user = await _userManager.GetUserAsync(principal);
			var role = new IdentityRole()
			{
				Name = "Admin"
			};
			if (!await _roleManager.RoleExistsAsync("Admin"))
			{
				await _roleManager.CreateAsync(role);
			}
			await _userManager.AddToRoleAsync(user, "Admin");
			return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string activityId)
        {
            repository.DeleteFromId(activityId);
            return RedirectToAction(nameof(MainPage));
        }

		[Authorize(Roles = "Admin")]
		public IActionResult Admin()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);

			return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
