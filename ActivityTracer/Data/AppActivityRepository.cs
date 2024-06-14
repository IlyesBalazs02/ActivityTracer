using ActivityTracer.Models;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ActivityTracer.Data
{
	public class AppActivityRepository : IAppActivityRepository
	{
		ActivityDbContext context;

		public AppActivityRepository(ActivityDbContext context)
		{
			this.context = context;
		}

		public void Create(AppActivity activity)
		{
			context.Activities.Add(activity);
			context.SaveChanges();
		}

		public IEnumerable<AppActivity> Read()
		{
			return context.Activities;
		}


		public AppActivity? ReadFromId(string id)
		{
			return context.Activities.FirstOrDefault(t => t.Id == id);
		}

		public void Update(AppActivity activity)
		{
			var old = ReadFromId((string)activity.Id);

			if (old != null)
			{
				throw new Exception("Object not found");
			}

			foreach (var prop in activity.GetType().GetProperties())
			{
				var value = prop.GetValue(activity);

				var oldProp = old.GetType().GetProperty(prop.Name);

				oldProp.SetValue(old, value);
			}

			context.SaveChanges();
		}

		public void DeleteFromId(string id)
		{
			var activity = ReadFromId(id);
			context.Activities.Remove(activity);
			context.SaveChanges();
		}
	}
}
