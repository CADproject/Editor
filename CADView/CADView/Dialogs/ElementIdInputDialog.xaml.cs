using System;
using System.Collections.Generic;
using System.Windows;

namespace CADView.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для OnePointDialog.xaml
    /// </summary>
    public partial class ElementIdInputDialog : IDataDialog
    {
        public ElementIdInputDialog()
        {
            InitializeComponent();
        }

        public List<object> Data { get; set; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Data = new List<object>();
            if (!string.IsNullOrEmpty(X1.Text))
            {
                foreach (string ids in X1.Text.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    uint id;
                    if (uint.TryParse(ids, out id))
                        Data.Add(id);
                }
            }

            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
