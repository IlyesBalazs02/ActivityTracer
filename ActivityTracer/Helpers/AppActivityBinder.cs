using ActivityTracer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Reflection;

namespace ActivityTracer.Helpers
{
	public class AppActivityBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			AppActivity activity = new AppActivity();


			//For the update(update view needs a hidden label for the id)
			if (bindingContext.ValueProvider.GetValue("Id").FirstValue is not null)
			{
				activity.Id = bindingContext.ValueProvider.GetValue("Id").FirstValue;
			}



			//Title
			activity.Title = bindingContext.ValueProvider.GetValue("Title").FirstValue;

			//SelectedSport
			if (Enum.TryParse(typeof(Sports), bindingContext.ValueProvider.GetValue("SelectedSport").FirstValue, out var selectedSportResult))
			{
				activity.SelectedSport = (Sports)selectedSportResult;
			}
			else
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			if(bindingContext.ValueProvider.GetValue("Description").FirstValue is not null)
			//Description
			activity.Description = bindingContext.ValueProvider.GetValue("Description").FirstValue;

			//Date
			if (DateTime.TryParse(bindingContext.ValueProvider.GetValue("Date").FirstValue, out DateTime dateTimeResult))
			{
				activity.Date = dateTimeResult;
			}
			else
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			//Time
			if (DateTime.TryParse(bindingContext.ValueProvider.GetValue("Time").FirstValue, out DateTime parsedDateTime))
			{
				int hours = parsedDateTime.Hour;
				int minutes = parsedDateTime.Minute;
				int seconds = parsedDateTime.Second;

				activity.Time = new TimeSpan(hours, minutes, seconds);
			}
			else
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			//Calories
			if (int.TryParse(bindingContext.ValueProvider.GetValue("Calories").FirstValue, out var caloriesResult))
			{
				activity.Calories = caloriesResult;
			}

			//Elevation
			if (int.TryParse(bindingContext.ValueProvider.GetValue("Elevation").FirstValue, out var elevationResult))
			{
				activity.Elevation = elevationResult;
			}

			//Pace
			if (DateTime.TryParseExact(bindingContext.ValueProvider.GetValue("Pace").FirstValue, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime paceResult))
			{
				activity.Pace = paceResult;
			}
			//else
			//{
			//	//activity.Pace = null; // If parsing fails, set it to null
			//	bindingContext.ModelState.AddModelError("Pace", "Invalid pace format. Expected format is HH:mm.");
			//}


			//Distance
			if (int.TryParse(bindingContext.ValueProvider.GetValue("Distance").FirstValue, out var distanceResult))
			{
				activity.Distance = distanceResult;
			}

			//AvgHeartRate
			if (int.TryParse(bindingContext.ValueProvider.GetValue("AvgHeartRate").FirstValue, out var avgHeartRateResult))
			{
				activity.AvgHeartRate = avgHeartRateResult;
			}

			//MaxHeartRate
			if (int.TryParse(bindingContext.ValueProvider.GetValue("MaxHeartRate").FirstValue, out var maxHeartRateResult))
			{
				activity.MaxHeartRate = maxHeartRateResult;
			}
			
			bindingContext.Result = ModelBindingResult.Success(activity);
			return Task.CompletedTask;


		}
	}
}
