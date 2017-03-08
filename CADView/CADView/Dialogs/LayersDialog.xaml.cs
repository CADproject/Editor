using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public LayerModel(int id)
        {
            _id = id;
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
        private readonly uint _documentId;

        public event PropertyChangedEventHandler PropertyChanged;

        public LayersDialogViewModel(ApplicationController controller, uint documentId)
        {
            _controller = controller;
            _documentId = documentId;
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

        public ObservableCollection<LayerModel> Layers { get; } = new ObservableCollection<LayerModel>()
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
            get
            {
                if (Layers.Count == 0) return null;
                return (Layers.First(lm => lm.Id == _controller.ActiveLayer) ?? Layers.First()) ?? new LayerModel();
            }
            set
            {
                if (value == null) return;
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

        public void Init()
        {
            Layers.Clear();
            foreach (var l in _controller.DocumentsLayers[_documentId])
            {
                Layers.Add(new LayerModel(l.Id) {Visible = l.Visible});
            }
            Layers.CollectionChanged+= delegate
            {
                _controller.DocumentsLayers[_documentId] =
                    Layers.Select(lm => new ApplicationController.CLayer() {Id = lm.Id, Visible = lm.Visible}).ToList();
            };
        }
    }

    /// <summary>
    /// Interaction logic for LayersDialog.xaml
    /// </summary>
    public partial class LayersDialog : ICallbackDialog
    {
        private static LayersDialog _instance;
        private readonly LayersDialogViewModel _viewModel;
        private bool _hidden;

        public LayersDialog(ApplicationController controller, uint documentId)
        {
            InitializeComponent();
            _instance = this;

            _viewModel = new LayersDialogViewModel(controller, documentId);

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

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (_hidden)
                _viewModel.Init();
            _hidden = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _hidden = true;
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
