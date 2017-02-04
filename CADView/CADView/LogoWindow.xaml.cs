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
            tickTimer.Start();

            Task initTask = new Task(() =>
            {
                Task.Delay(1000).Wait();
            });

            initTask.Start();
            await initTask;

            new MainWindow().Show();
            Close();
        }
    }
}
