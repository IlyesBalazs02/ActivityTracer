using ActivityTracer.Data;
using ActivityTracer.Hubs;
using ActivityTracer.Models;
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
		public IEnumerable<AppActivity> getActivities()
		{
			return repository.Read();
		}

		[HttpGet("{id}")]
		public AppActivity? getActivities(string id)
		{
			return repository.ReadFromId(id);
		}

		[HttpPost]
		public async void AddActivity(AppActivity c)
		{
			repository.Create(c);
		}

		[HttpPut]
		public void EditActivity([FromBody] AppActivity c)
		{
			repository.Update(c);
		}

		[HttpDelete("{id}")]
		public void DeleteCar(string id)
		{
			repository.DeleteFromId(id);
		}
	}
}
