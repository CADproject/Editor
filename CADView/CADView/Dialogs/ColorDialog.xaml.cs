using System;
using System.Collections.Generic;
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

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if(_busy) return;
            if (((Button) sender).Tag is Color)
            {
                _busy = true;
                DataChanged?.Invoke(new List<object>() {((Button) sender).Tag}, EventArgs.Empty);
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

        public event EventHandler DataChanged;

        public void DataProcessComplete()
        {
            _busy = false;
        }

        public static ColorDialog Instance => _instance ?? (_instance = new ColorDialog());
    }
}
