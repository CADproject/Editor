using System.Collections.Generic;
using System.Windows;

namespace CADView.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для TwoPointDialog.xaml
    /// </summary>
    public partial class TwoPointDialog : Window, IDataDialog
    {
        public TwoPointDialog()
        {
            InitializeComponent();
        }

        ~TwoPointDialog()
        {
            
        }

        public List<object> Data { get; set; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Data = new List<object>(2);
            Data.Add(double.Parse(X1.Text));
            Data.Add(double.Parse(Y1.Text));
            Data.Add(double.Parse(X2.Text));
            Data.Add(double.Parse(Y2.Text));

            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
