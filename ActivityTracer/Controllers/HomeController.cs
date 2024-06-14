using ActivityTracer.Data;
using ActivityTracer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ActivityTracer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IAppActivityRepository repository;

		public HomeController(ILogger<HomeController> logger, IAppActivityRepository repository)
		{
			_logger = logger;
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
        public IActionResult Create(AppActivity appActivity)
        {
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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
