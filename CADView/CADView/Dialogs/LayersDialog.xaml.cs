using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CADView.Dialogs
{
    internal class LayersDialogViewModel : INotifyPropertyChanged
    {
        public event EventHandler DataChanged;
        private RelayCommand _addLayer;
        private RelayCommand _selectLayer;
        private static LayersDialogViewModel _instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public static LayersDialogViewModel GetLayersViewModel()
        {
            return _instance ?? (_instance = new LayersDialogViewModel());
        }

        private LayersDialogViewModel()
        {
            _instance = this;
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<uint> Layers { get; } = new ObservableCollection<uint>() {0};

        public static ObservableCollection<uint> SelectedLayers { get; } = new ObservableCollection<uint>() {0};

        public RelayCommand AddLayer
        {
            get { return _addLayer ?? (_addLayer = new RelayCommand(obj => Layers.Add(Layers.Last() + 1))); }
        }

        public RelayCommand SelectLayer
        {
            get
            {
                return _selectLayer ?? (_selectLayer = new RelayCommand(
                    obj => DataChanged?.Invoke(SelectedLayers.Select(u => (object) u).ToList(), EventArgs.Empty)));
            }
        }
    }

    /// <summary>
    /// Interaction logic for LayersDialog.xaml
    /// </summary>
    public partial class LayersDialog : Window, ICallbackDialog
    {
        private static LayersDialog _instance;

        private LayersDialog()
        {
            InitializeComponent();

            LayersDialogViewModel viewModel = LayersDialogViewModel.GetLayersViewModel();
            viewModel.DataChanged += delegate(object sender, EventArgs args) { DataChanged?.Invoke(sender, args); };
            DataContext = viewModel;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            CancelButtonClick(this, null);
        }

        public static LayersDialog Instance => _instance ?? (_instance = new LayersDialog());

        public event EventHandler DataChanged;
    }
}
