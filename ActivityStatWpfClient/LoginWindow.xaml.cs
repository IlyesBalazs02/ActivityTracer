using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ActivityStatWpfClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

		private async void Button_Click(object sender, RoutedEventArgs e)
        {
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("http://localhost:7016");
			client.DefaultRequestHeaders.Accept.Add(
				new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
			);

			var response = await client.PostAsJsonAsync<LoginViewModel>("auth", new LoginViewModel()
			{
				Email = tb_Email.Text,
				Password = tb_password.Password
			});

			var token = await response.Content.ReadAsAsync<TokenModel>();

			MessageBox.Show(token.Token);

			//MainWindow mw = new MainWindow();

			//         mw.ShowDialog();
		}

	}

	internal class TokenModel
	{
		public string Token { get; set; }

	}

	internal class LoginViewModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
