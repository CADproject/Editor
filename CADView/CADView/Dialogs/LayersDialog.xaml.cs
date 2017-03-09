using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using CADController;

namespace CADView.Dialogs
{
    public class LayerModel : Layer, INotifyPropertyChanged
    {
        public LayerModel(bool visible = false) : base(visible)
        {
        }

        public LayerModel(int id, bool visible = false) : base(id, visible)
        {
        }

        public LayerModel(Layer layer) : base(layer)
        {

        }

        public override int Id
        {
            get { return base.Id; }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
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

    internal class LayersDialogViewModel : INotifyPropertyChanged, IDisposable
    {
        public event MainWindowViewModel.ProcessDataDelegate DataChanged;
        private RelayCommand _addLayer;
        private bool _ready = true;
        private readonly ApplicationController _controller;
        private readonly uint _documentId;
        private bool _disposed;

        public event PropertyChangedEventHandler PropertyChanged;

        public LayersDialogViewModel(ApplicationController controller, uint documentId)
        {
            _controller = controller;
            _documentId = documentId;
            LayerModel.VisibleChangedStatic+=LayerModelOnVisibleChangedStatic;
        }

        ~LayersDialogViewModel()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
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

        public ObservableCollection<Layer> Layers
        {
            get { return _controller.Documents[_documentId].Layers; }
        }

        public Layer ActiveLayer
        {
            get { return _controller.Documents[_documentId].ActiveLayer; }
            set
            {
                _controller.Documents[_documentId].ActiveLayer = value;
                OnPropertyChanged();
            }
        }

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
        private LayersDialogViewModel _viewModel;

        public LayersDialog(ApplicationController controller, uint documentId)
        {
            InitializeComponent();

            _viewModel = new LayersDialogViewModel(controller, documentId);

            _viewModel.DataChanged += OnDataChanged;
            DataContext = _viewModel;
        }

        private Task OnDataChanged(object[] sender)
        {
            return DataChanged?.Invoke(sender);
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.DataChanged -= OnDataChanged;
            _viewModel.Dispose();
            _viewModel = null;
            DataContext = null;
            DialogResult = false;
            //GC.Collect();
        }

        public event MainWindowViewModel.ProcessDataDelegate DataChanged;

        public void DataProcessComplete()
        {
            _viewModel.Ready = true;
        }
    }
}
