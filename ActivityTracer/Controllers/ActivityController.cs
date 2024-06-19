using ActivityTracer.Data;
using ActivityTracer.Hubs;
using ActivityTracer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ActivityTracer.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class ActivityController : ControllerBase
	{
		IAppActivityRepository repository;


		public ActivityController(IAppActivityRepository repository)
		{
			this.repository = repository;
		}

		[HttpGet]
		[Authorize]
		public IEnumerable<AppActivity> getActivities()
		{
			return repository.Read();
		}

		[HttpGet("{id}")]
		[Authorize]
		public AppActivity? getActivities(string id)
		{
			return repository.ReadFromId(id);
		}

		[HttpPost]
		[Authorize]
		public async void AddActivity(AppActivity c)
		{
			repository.Create(c);
		}

		[HttpPut]
		[Authorize]
		public void EditActivity([FromBody] AppActivity c)
		{
			repository.Update(c);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public void DeleteCar(string id)
		{
			repository.DeleteFromId(id);
		}
	}
}
