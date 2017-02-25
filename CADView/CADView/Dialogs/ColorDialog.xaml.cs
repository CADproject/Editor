using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CADController;

namespace CADView.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ColorDialog.xaml
    /// </summary>
    public partial class ColorDialog : Window, IDataDialog
    {
        private List<object> _data;

        public ColorDialog()
        {
            InitializeComponent();
        }

        public List<object> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Data = new List<object>();

            if (((Button) sender).Tag is Color)
            {
                Data.Add(((Button) sender).Tag);
                this.DialogResult = true;
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OkButtonClick(sender, e);
        }
    }
}
