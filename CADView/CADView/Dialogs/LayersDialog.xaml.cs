using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace CADView.Dialogs
{
    internal class LayersDialogViewModel : INotifyPropertyChanged
    {
        public event EventHandler DataChanged;
        private RelayCommand _addLayer;
        private RelayCommand _selectLayer;
        private bool _ready = true;
        private static LayersDialogViewModel _instance;

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

        public static ObservableCollection<ListBoxItem> Layers { get; } = new ObservableCollection<ListBoxItem>() {new ListBoxItem() {Content = (uint)0, IsSelected = true} };

        public IList SelectedLayers { get; set; }

        public RelayCommand AddLayer
        {
            get
            {
                return _addLayer ?? (_addLayer = new RelayCommand(
                    obj => Layers.Add(new ListBoxItem() {Content = (uint) Layers.Last().Content + 1})));
            }
        }

        public RelayCommand SelectLayer
        {
            get
            {
                return _selectLayer ?? (_selectLayer = new RelayCommand(
                    obj =>
                    {
                        Ready = false;
                        DataChanged?.Invoke(SelectedLayers.Cast<ListBoxItem>().Select(u => u.Content).ToList(), EventArgs.Empty);
                    }));
            }
        }

        public bool Ready
        {
            get { return _ready; }
            set
            {
                _ready = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Interaction logic for LayersDialog.xaml
    /// </summary>
    public partial class LayersDialog : ICallbackDialog
    {
        private static LayersDialog _instance;
        readonly LayersDialogViewModel _viewModel;

        private LayersDialog()
        {
            InitializeComponent();

            _viewModel = LayersDialogViewModel.GetLayersViewModel();
            _viewModel.SelectedLayers = LayersList.SelectedItems;
            LayersList.SelectedItems.Add(1);
            _viewModel.DataChanged += delegate(object sender, EventArgs args)
            {
                DataChanged?.Invoke(sender, args);
            };
            DataContext = _viewModel;
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

        public void DataProcessComplete()
        {
            _viewModel.Ready = true;
        }
    }
}
