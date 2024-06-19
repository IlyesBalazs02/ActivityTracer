using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActivityStatWpfClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<AppActivity> AppActivities { get; set; }


		private AppActivity actualActivity;

		HttpClient client;

		HubConnection conn;

		public AppActivity ActualActivity
		{
			get { return actualActivity; }
			set { actualActivity = value.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActualActivity")); }
		}

		public MainWindow()
		{

			InitializeComponent();

			client = new HttpClient();
			client.BaseAddress = new Uri("https://localhost:7016/");
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(
			  new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			Task.Run(async () =>
			{
				AppActivities = new ObservableCollection<AppActivity>(await GetActivities());
			}).Wait();

			conn = new HubConnectionBuilder().WithUrl("https://localhost:7016/events").Build();
			conn.Closed += async (error) =>
			{
				await Task.Delay(new Random().Next(0, 5) * 1000);
				await conn.StartAsync();
			};

			conn.On<AppActivity>("activityCreated", async t => await Refresh());
			conn.On<string>("activityModified", async t => await Refresh());
			conn.On<string>("activityDeleted", t => 
			{
				var activityToDelete = AppActivities.FirstOrDefault(X => X.Id == t);
				this.Dispatcher.Invoke(() =>
				{
					AppActivities.Remove(activityToDelete);
				});
			});

			conn.On<AppActivity>("activityModified", async (updatedActivity) =>
			{
				// Handle the updated activity as needed
				await Refresh(); // Ensure Refresh() method updates UI or data binding
			});

			Task.Run(async () =>
			{
				await conn.StartAsync();
			}).Wait();

			

			// Ensure connection starts properly

			this.DataContext = this;
		}

		async Task Refresh()
		{
			AppActivities = new ObservableCollection<AppActivity>(await GetActivities());
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AppActivities"));
		}

		async Task<IEnumerable<AppActivity>> GetActivities()
		{
			var response = await client.GetAsync("/Activity");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsAsync<IEnumerable<AppActivity>>();
			}
			throw new Exception("something wrong...");
		}
	}
}