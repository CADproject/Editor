using System.Collections.Generic;
using System.Windows;

namespace CADView.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для OnePointDialog.xaml
    /// </summary>
    public partial class OnePointDialog : Window, IDataDialog
    {
        public OnePointDialog()
        {
            InitializeComponent();
        }

        ~OnePointDialog()
        {
            
        }

        public List<object> Data { get; set; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Data = new List<object>(2);
            Data.Add(double.Parse(X1.Text));
            Data.Add(double.Parse(Y1.Text));

            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
