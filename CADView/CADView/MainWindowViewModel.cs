using System;
using System.Collections.Generic;
using System.ComponentModel;
using CADController;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using CADView.Dialogs;
using Application = System.Windows.Application;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using System.Threading.Tasks;

namespace CADView
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public

        public MainWindowViewModel()
        {
            Controller = new ApplicationController();
            RenderPanel.Loaded += RenderPanelOnLoad;
            RenderPanel.Resized += RenderPanelOnResize;
            RenderPanel.Rendered += RenderPanelOnRender;
            RenderPanel.MouseFired += RenderPanelOnMouseFire;
        }

        ~MainWindowViewModel()
        {
            var ids = DocumentViewModels.Select(d => d.Key).ToList();
            foreach (var id in ids)
            {
                DocumentViewModels[id].Dispose();
                Controller.finalDocument(Session, id);
            }
            Controller.finalSession(Session);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate Task ProcessDataDelegate(object[] o);

        internal ApplicationController Controller { get; private set; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        public void Init()
        {
            Session = Controller.initSession();
            _inited = true;

            _timer = new DispatcherTimer(DispatcherPriority.Normal, Application.Current.Dispatcher);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _timer.Tick += delegate
            {
                if (DocumentViewModels.Count == 0) return;
                Controller.draw(Session, ActiveDocument.DocumentID);
            };
            _timer.Start();
        }

        public void CreateDocument()
        {
            var model = new DocumentModel(new LayerModel(true));
            var host = new WindowsFormsHost()
            {
                Child = new RenderPanel(model),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            TabItem tab = new TabItem()
            {
                Content = host,
                DataContext = model,
            };
            DocumentViewModelsTabs.Add(tab);
            _tabsDocuments[tab] = model;
            _selectedDocumentIndex = DocumentViewModelsTabs.Count - 1;
            OnPropertyChanged("SelectedDocumentIndex");
        }

        public int SelectedDocumentIndex
        {
            get { return _selectedDocumentIndex; }
            set
            {
                _selectedDocumentIndex = value;
                OnPropertyChanged();

                if (SelectedDocumentIndex < 0 || SelectedDocumentIndex > DocumentViewModelsTabs.Count) return;

                var size = ((WindowsFormsHost) DocumentViewModelsTabs[SelectedDocumentIndex].Content)
                    .Child.Size;
                Controller.activateDocement(Session, ActiveDocument.DocumentID, size.Width,
                    size.Height);
            }
        }

        public ObservableCollection<TabItem> DocumentViewModelsTabs
        {
            get { return _documentViewModelsTabs; }
            set
            {
                _documentViewModelsTabs = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<uint, Document> DocumentViewModels
        {
            get { return Controller.Documents; }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                OnPropertyChanged();
            }
        }

        public double WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                _windowHeight = value;
                OnPropertyChanged();
            }
        }

        public Document ActiveDocument
        {
            get
            {
                if (SelectedDocumentIndex < 0) return null;
                return _tabsDocuments[DocumentViewModelsTabs[SelectedDocumentIndex]];
            }
            set
            {
                SelectedDocumentIndex =
                    DocumentViewModelsTabs.IndexOf(_tabsDocuments.FirstOrDefault(i => i.Value == value).Key);
            }
        }

        #endregion

        #region Protected

#if OLDDOTNET
        protected virtual void OnPropertyChanged(string propertyName)
#else
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
#endif
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private

        private uint _session;
        bool _inited;
        private bool _isActive;
        private RelayCommand _documentWorkCommand;
        private RelayCommand _controllerWorkCommand;
        private RelayCommand _controllerDialogCommand;
        private RelayCommand _closeDocumentCommand;
        private ObservableCollection<TabItem> _documentViewModelsTabs = new ObservableCollection<TabItem>();
        private int _selectedDocumentIndex;
        private DispatcherTimer _timer;
        private double _windowWidth;
        private double _windowHeight;
        private readonly List<DocumentModel> _documentViewModels = new List<DocumentModel>();
        private RelayCommand _closeApplicationCommand;
        private readonly Dictionary<TabItem, Document> _tabsDocuments = new Dictionary<TabItem, Document>(); 

        private uint Session
        {
            get
            {
                if (!_inited)
                    throw new ApplicationException("No active session.");
                return _session;
            }
            set { _session = value; }
        }

        private void ProcessDocumentWork(object obj)
        {
            CreateDocument();
        }

        private void RenderPanelOnLoad(IntPtr hwnd, int w, int h, Document model)
        {
            var activeDocument = Controller.initDocument(Session, hwnd, model);
            Controller.activateDocement(Session, activeDocument, w, h);
            ActiveDocument.Title = "Document # " + activeDocument;
            ActiveDocument.DocumentID = activeDocument;
            IsActive = true;
        }

        private void RenderPanelOnResize(int w, int h)
        {
            Controller.resizeDocument(Session, ActiveDocument.DocumentID, w, h);
        }

        private void RenderPanelOnRender()
        {
            if (DocumentViewModels.Count == 0) return;
            Controller.draw(Session, ActiveDocument.DocumentID);
        }

        private void RenderPanelOnMouseFire(MouseEventArgs args)
        {
            Controller.eventHendling(ActiveDocument.DocumentID, (int) args.Button, args.X,
                args.Y, args.Delta);
        }

        private async Task<bool> ProcessControllerWork(ApplicationController.Operations type, object data)
        {
            try
            {
                IsActive = false;
                await Task.Run(delegate
                {
                    Controller.procOperation(Session, ActiveDocument.DocumentID, type,
                        (object[]) data);
                });
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception");
                return false;
            }
            finally
            {
                IsActive = true;
            }
        }

        private async void ProcessControllerRaiseDialog(object obj)
        {
            ApplicationController.Operations type = (ApplicationController.Operations) obj;

            Window modalWindow = null;
            Window separatedWindow = null;

            switch (type)
            {
                case ApplicationController.Operations.OpPointCreate:
                    modalWindow = new OnePointDialog();
                    modalWindow.Title = "Create Point";
                    break;
                case ApplicationController.Operations.OpLineCreate:
                    modalWindow = new TwoPointDialog();
                    modalWindow.Title = "Create Line";
                    break;
                case ApplicationController.Operations.OpCircleCreate:
                    modalWindow = new TwoPointDialog();
                    modalWindow.Title = "Create Circle";
                    break;
                case ApplicationController.Operations.OpContourCreate:
                    modalWindow = new ElementIdInputDialog();
                    modalWindow.Title = "Create Contour from objects";
                    break;
                case ApplicationController.Operations.OpDeleteObject:
                    modalWindow = new DestroyObjectDialog();
                    modalWindow.Title = "Delete object";
                    break;
                case ApplicationController.Operations.OpDestroyContour:
                    modalWindow = new DestroyContourDialog();
                    modalWindow.Title = "Destroy Contour";
                    break;
                case ApplicationController.Operations.OpSetBackgroundColor:
                    separatedWindow = Dialogs.ColorDialog.Instance;
                    separatedWindow.Title = "Colors manager";
                    break;
                case ApplicationController.Operations.OpSetLayersToShow:
                    separatedWindow = new LayersDialog(Controller, ActiveDocument.DocumentID);
                    separatedWindow.Title = "Layers manager";
                    break;
            }

            if (modalWindow is IDataDialog)
            {
                modalWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (modalWindow.ShowDialog() == true)
                {
                    {
                        var data = ((IDataDialog) modalWindow).Data.ToArray();
                        await ProcessControllerWork(type, data);
                    }

                }
            }
            if (separatedWindow is ICallbackDialog)
            {
                separatedWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ((ICallbackDialog) separatedWindow).DataChanged += o => ProcessControllerWork(type, o);
                separatedWindow.ShowDialog();
            }
        }

        #endregion

        #region Commands

        public RelayCommand DocumentWorkCommand
        {
            get
            {
                return _documentWorkCommand ?? (_documentWorkCommand = new RelayCommand(ProcessDocumentWork,
                    o => (DocumentViewModels.Count > 0 && IsActive) || DocumentViewModels.Count == 0));
            }
        }

        public RelayCommand ControllerWorkCommand
        {
            get
            {
                return _controllerWorkCommand ?? (_controllerWorkCommand = new RelayCommand(async delegate(object o)
                {
                    await ProcessControllerWork((ApplicationController.Operations) o, null);
                }));
            }
        }

        public RelayCommand ControllerDialogCommand
        {
            get
            {
                return _controllerDialogCommand ??
                       (_controllerDialogCommand = new RelayCommand(ProcessControllerRaiseDialog));
            }
        }

        public RelayCommand CloseApplicationCommand
        {
            get
            {
                return _closeApplicationCommand ??
                       (_closeApplicationCommand = new RelayCommand(o => App.Current.Shutdown()));
            }
        }

        public RelayCommand CloseDocumentCommand
        {
            get
            {
                return _closeDocumentCommand ??
                       (_closeDocumentCommand = new RelayCommand(delegate(object i)
                       {
                           string name = i as string;
                           if(string.IsNullOrEmpty(name)) return;

                           var tab = DocumentViewModelsTabs.ToList().Find(d => d.Header.Equals(name));
                           var document = _tabsDocuments[tab];

                           if(document == null) return;

                           var host = (WindowsFormsHost) tab.Content;
                           tab.Content = null;
                           tab.DataContext = null;
                           DocumentViewModelsTabs.Remove(tab);
                           _tabsDocuments.Remove(tab);
                           host.Child.Dispose();
                           host.Dispose();
                           document.Dispose();
                           Controller.finalDocument(Session, document.DocumentID);
                           SelectedDocumentIndex = SelectedDocumentIndex;
                           IsActive = DocumentViewModels.Count > 0;
                       }));
            }
        }

        #endregion
    }
}
