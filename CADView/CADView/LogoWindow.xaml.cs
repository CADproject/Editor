using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CADView
{
    /// <summary>
    /// Interaction logic for LogoWindow.xaml
    /// </summary>
    public partial class LogoWindow
    {
        public LogoWindow()
        {
            InitializeComponent();
        }
        
        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            var iconStream =
                Application.GetResourceStream(new System.Uri("pack://application:,,,/Icons/logo.png")).Stream;

            Icon = BitmapFrame.Create(iconStream);

            var main = new MainWindow();
            MainWindowViewModel view = (MainWindowViewModel) main.DataContext;

            main.Icon =BitmapFrame.Create(iconStream);

            await Task.Run(delegate
            {
                view.Init();
            });

            main.Show();
            Close();
        }
    }
}
