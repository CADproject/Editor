using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using CADController;

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

        public static ObservableCollection<uint> Layers { get; } = new ObservableCollection<uint>() { 0 };

        public IList SelectedLayers { get; set; }

        public RelayCommand AddLayer
        {
            get
            {
                return _addLayer ?? (_addLayer = new RelayCommand(
                    obj =>
                    {
                        Layers.Add(Layers.Last() + 1);
                    }));
            }
        }

        public uint ActiveLayer
        {
            get { return Controller.ActiveLayer; }
            set
            {
                Controller.ActiveLayer = value;
                OnPropertyChanged();
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
                        DataChanged?.Invoke(SelectedLayers.Cast<uint>().Select(u => u).ToList(), EventArgs.Empty);
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

        public ApplicationController Controller { get; set; }
    }

    /// <summary>
    /// Interaction logic for LayersDialog.xaml
    /// </summary>
    public partial class LayersDialog : ICallbackDialog, IControlledDialog
    {
        private static LayersDialog _instance;
        private readonly LayersDialogViewModel _viewModel;

        private LayersDialog()
        {
            InitializeComponent();

            _viewModel = LayersDialogViewModel.GetLayersViewModel();
            _viewModel.SelectedLayers = LayersList.SelectedItems;
            _viewModel.Controller = Controller;
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

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LayersList.Focus();
        }

        public static LayersDialog Instance => _instance ?? (_instance = new LayersDialog());

        public event EventHandler DataChanged;

        public void DataProcessComplete()
        {
            _viewModel.Ready = true;
        }

        public ApplicationController Controller
        {
            get { return _viewModel.Controller; }
            set { _viewModel.Controller = value; }
        }
    }
}
