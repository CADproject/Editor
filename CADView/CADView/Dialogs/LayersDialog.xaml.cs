using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using CADController;

namespace CADView.Dialogs
{
    internal class LayerModel : INotifyPropertyChanged
    {
        private int _id;
        private bool _visible;
        private static int _counter;

        public LayerModel()
        {
            _id = _counter++;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged();
                VisibleChangedStatic?.Invoke(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public static event PropertyChangedEventHandler VisibleChangedStatic;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class LayersDialogViewModel : INotifyPropertyChanged
    {
        public event MainWindowViewModel.ProcessDataDelegate DataChanged;
        private RelayCommand _addLayer;
        private bool _ready = true;
        private readonly ApplicationController _controller;

        public event PropertyChangedEventHandler PropertyChanged;

        public LayersDialogViewModel(ApplicationController controller)
        {
            _controller = controller;
            LayerModel.VisibleChangedStatic+=LayerModelOnVisibleChangedStatic;
        }

        ~LayersDialogViewModel()
        {
            LayerModel.VisibleChangedStatic -= LayerModelOnVisibleChangedStatic;
        }

        private void LayerModelOnVisibleChangedStatic(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Ready = false;
            DataChanged?.Invoke(Layers.Where(lm => lm.Visible).Select(lm => (object)lm.Id).ToArray());
            Ready = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<LayerModel> Layers { get; } = new ObservableCollection<LayerModel>()
        {
            new LayerModel() {Visible = true}
        };

        public RelayCommand AddLayer
        {
            get
            {
                return _addLayer ?? (_addLayer = new RelayCommand(
                    obj =>
                    {
                        Layers.Add(new LayerModel());
                    }));
            }
        }

        public LayerModel ActiveLayer
        {
            get { return (Layers.First(lm => lm.Id == _controller.ActiveLayer) ?? Layers.First()) ?? new LayerModel(); }
            set
            {
                _controller.ActiveLayer = value.Id;
                OnPropertyChanged();
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
        private readonly LayersDialogViewModel _viewModel;

        public LayersDialog(ApplicationController controller)
        {
            InitializeComponent();
            _instance = this;

            _viewModel = new LayersDialogViewModel(controller);

            _viewModel.DataChanged += sender => DataChanged?.Invoke(sender);
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

        public static LayersDialog Instance => _instance;

        public event MainWindowViewModel.ProcessDataDelegate DataChanged;

        public void DataProcessComplete()
        {
            _viewModel.Ready = true;
        }
    }
}
