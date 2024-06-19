using ActivityTracer.Hubs;
using ActivityTracer.Models;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;

namespace ActivityTracer.Data
{
	public class AppActivityRepository : IAppActivityRepository
	{
		ActivityDbContext context;
		IHubContext<EventHub> hub;

		public AppActivityRepository(ActivityDbContext context, IHubContext<EventHub> hub)
		{
			this.context = context;
			this.hub = hub;
		}

		public async void Create(AppActivity activity)
		{
			context.Activities.Add(activity);
			context.SaveChanges();

			await hub.Clients.All.SendAsync("activityCreated", activity);
		}

		public IEnumerable<AppActivity> Read()
		{
			return context.Activities;
		}


		public AppActivity? ReadFromId(string id)
		{
			return context.Activities.FirstOrDefault(t => t.Id == id);
		}

		public async void Update(AppActivity activity)
		{
			var old = ReadFromId((string)activity.Id);
			
			if (old is null)
			{
				throw new Exception("Object not found");
			}

			foreach (var prop in activity.GetType().GetProperties().Where( t => t.Name != "Id" && t.Name != "PhotoUrl"))
			{
					var value = prop.GetValue(activity);

					var oldProp = old.GetType().GetProperty(prop.Name);

					oldProp.SetValue(old, value);
			}

			
			context.SaveChanges();

			await hub.Clients.All.SendAsync("activityModified", old.Id);
		}

		public async void DeleteFromId(string id)
		{
			var activity = ReadFromId(id);
			context.Activities.Remove(activity);
			context.SaveChanges();

			await hub.Clients.All.SendAsync("activityDeleted", id);
		}
	}
}
