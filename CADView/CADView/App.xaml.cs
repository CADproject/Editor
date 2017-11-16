using System.Windows;

namespace CADView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Instance = this;
        }

        public static App Instance { get; private set; }
    }
}
