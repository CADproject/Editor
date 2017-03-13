using System.Threading.Tasks;
using System.Windows;

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
            var main = new MainWindow();
            MainWindowViewModel view = (MainWindowViewModel) main.DataContext;

            await Task.Run(delegate
            {
                view.Init();
            });

            main.Show();
            Close();
        }
    }
}
