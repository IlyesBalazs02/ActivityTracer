using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ActivityClientWpf
{


	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<AppActivity> AppActivities { get; set; }


		private AppActivity actualActivity;

		public AppActivity ActualActivity
		{
			get { return actualActivity; }
			set { actualActivity = value.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActualActivity")); }
		}

		public MainWindow()
		{
			InitializeComponent();

			sportList.ItemsSource = Enum.GetValues(typeof(Sports)).Cast<Sports>();

			AppActivities = new ObservableCollection<AppActivity>();
			AppActivities.Add(new AppActivity() { Title = "Title1", Description = "Desc1", SelectedSport = Sports.Running, Date = DateTime.Now, Time = "00:30:22" });
			AppActivities.Add(new AppActivity() { Title = "Title2", Description = "Desc2", SelectedSport = Sports.Climbing, Date = DateTime.Now, Time = "00:30:22" });
			AppActivities.Add(new AppActivity() { Title = "Title3", Description = "Desc3", SelectedSport = Sports.Hiking, Date = DateTime.Now, Time = "00:30:22" });
			this.DataContext = this;
		}

	}
}