using System.Collections.Generic;
using System.Windows;

namespace CADView.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для DestroyObject.xaml
    /// </summary>
    public partial class DestroyContourDialog : IDataDialog
    {
        public DestroyContourDialog()
        {
            InitializeComponent();
        }

        public List<object> Data { get; set; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Data = new List<object>(2);
            Data.Add(double.Parse(IdTextBox.Text));

            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
