using System.Threading.Tasks;
using System.Windows;
using Timer = System.Timers.Timer;

namespace CADView
{
    /// <summary>
    /// Interaction logic for LogoWindow.xaml
    /// </summary>
    public partial class LogoWindow : Window
    {
        public LogoWindow()
        {
            InitializeComponent();
        }

        readonly Timer tickTimer = new Timer();

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            //tickTimer.Start();
            var main = new MainWindow();
            MainWindowViewModel view = (MainWindowViewModel) main.DataContext;

            Task initTask = new Task(() =>
            {
                view.Init();
            });

            initTask.Start();
            await initTask;

            main.Show();
            Close();
        }
    }
}
