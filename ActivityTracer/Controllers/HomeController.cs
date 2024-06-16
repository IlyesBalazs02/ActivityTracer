using ActivityTracer.Data;
using ActivityTracer.Models;
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
        IAppActivityRepository repository;

        public HomeController(ILogger<HomeController> logger, UserManager<SiteUser> userManager, IAppActivityRepository repository)
        {
            _logger = logger;
            _userManager = userManager;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            var activity = new AppActivity();
            return View(activity);
        }

        [HttpPost]
        public IActionResult Create([Bind()] AppActivity appActivity)
        {
            appActivity.OwnerId = _userManager.GetUserId(this.User);

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

        public IActionResult Update(string id)
        {
            var appActivity = repository.ReadFromId(id);
            return View(appActivity);
        }

        [HttpPost]
        public IActionResult Update(AppActivity appActivity)
        {
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

        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            user.ContentType = "contenttypeisokay";
            user.FirstName = "firstnameisokay";
			var result = await _userManager.UpdateAsync(user);

			return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
