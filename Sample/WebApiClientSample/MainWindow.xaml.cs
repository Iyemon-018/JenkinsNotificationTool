using System.Windows;

namespace WebApiClientSample
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<string> GetRequest(int id)
        {
            var client = new HttpClient();
            using (var response = await client.GetAsync($"http://weather.livedoor.com/forecast/webservice/json/v1?city={id}"))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        private async void GetResponseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var request = await GetRequest(400040);
            GetResponseTextBox.Text = request;
        }
    }
}
