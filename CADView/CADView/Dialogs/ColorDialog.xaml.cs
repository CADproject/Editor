using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CADController;

namespace CADView.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ColorDialog.xaml
    /// </summary>
    public partial class ColorDialog : ICallbackDialog
    {
        private static ColorDialog _instance;

        private ColorDialog()
        {
            InitializeComponent();
        }

        private bool _busy;

        private async void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if(_busy) return;
            if (((Button) sender).Tag is Color)
            {
                _busy = true;
                if (DataChanged != null)
                    await DataChanged.Invoke(new[] {((Button) sender).Tag});
                _busy = false;
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OkButtonClick(sender, e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            CancelButtonClick(this, null);
        }

        public event MainWindowViewModel.ProcessDataDelegate DataChanged;

        public static ColorDialog Instance => _instance ?? (_instance = new ColorDialog());
    }
}
