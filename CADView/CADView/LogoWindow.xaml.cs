using System.Threading.Tasks;
using System.Windows;
#if OLDDOTNET
using System.Threading;
#endif

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


#if OLDDOTNET
        private readonly EventWaitHandle _awaiter = new EventWaitHandle(false, EventResetMode.AutoReset);
#endif

#if OLDDOTNET
        private void OnLoad(object sender, RoutedEventArgs e)
#else
        private async void OnLoad(object sender, RoutedEventArgs e)
#endif
        {
            //tickTimer.Start();
            var main = new MainWindow();
            MainWindowViewModel view = (MainWindowViewModel) main.DataContext;

            Task initTask = new Task(() =>
            {
                view.Init();
#if OLDDOTNET
                _awaiter.Set();
#endif
            });

            initTask.Start();

#if OLDDOTNET
            _awaiter.WaitOne();
#else
            await initTask;
#endif

            main.Show();
            Close();
        }
    }
}
