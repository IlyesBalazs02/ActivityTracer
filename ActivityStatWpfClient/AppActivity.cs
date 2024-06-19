using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityStatWpfClient
{
	public enum Sports
	{
		Activity,
		Workout,
		Running,
		Hiking,
		Climbing,
		Mountaineering
	}

	public class AppActivity : INotifyPropertyChanged
	{
		private string id;

		public string Id
		{
			get { return id; }
			set { id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id")); }
		}

		private string title;

		public string Title
		{
			get { return title; }
			set { title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title")); }
		}

		private string description;

		public string Description
		{
			get { return description; }
			set { description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description")); }
		}

		private Sports selectedSport;

		public Sports SelectedSport
		{
			get { return selectedSport; }
			set { selectedSport = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedSport")); }
		}

		private DateTime date;

		public DateTime Date
		{
			get { return date; }
			set { date = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date")); }
		}

		private TimeSpan time;

		public string Time
		{
			get { return time.ToString(@"hh\:mm\:ss"); }
			set
			{
				if (TimeSpan.TryParse(value, out var newTime))
				{
					time = newTime;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
				}
			}
		}

		public AppActivity()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public AppActivity GetCopy()
		{
			return new AppActivity()
			{
				Id = this.Id,
				Title = this.Title,
				Description = this.Description,
				SelectedSport = this.SelectedSport,
				Date = this.Date,
				Time = this.Time
			};
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
