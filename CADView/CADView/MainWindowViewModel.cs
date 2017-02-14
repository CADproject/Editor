﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using CADController;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using CADView.Dialogs;
#if !OLDDOTNET
using System.Threading.Tasks;
#endif

namespace CADView
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
#region Public

        public MainWindowViewModel()
        {
            Controller = new ApplicationController();
            RenderPanel.Loaded += RenderPanelOnLoaded;
            RenderPanel.Resized += RenderPanelOnResize;
        }

        ~MainWindowViewModel()
        {
            foreach (var documentViewModel in DocumentViewModels)
            {
                //Controller.finalDocument(Session, ((DocumentViewModel)documentViewModel.DataContext).DocumentID);
            }
            Controller.finalSession(Session);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal ApplicationController Controller { get; private set; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public void Init()
        {
            Session = Controller.initSession();
            _inited = true;

            _timer = new DispatcherTimer(DispatcherPriority.Normal, Application.Current.Dispatcher);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _timer.Tick+= delegate
            {
                if(DocumentViewModels.Count == 0) return;
                Controller.draw(Session, DocumentViewModels[SelectedDocumentIndex].DocumentID);
            };
            _timer.Start();
        }

        public void CreateDocument()
        {
            var host = new WindowsFormsHost()
            {
                Child = new RenderPanel(),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Grid grid = new Grid();
            grid.Children.Add(host);
            Binding binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("WindowWidth"),
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            var r = BindingOperations.SetBinding(grid, FrameworkElement.WidthProperty, binding);
            binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("WindowHeight"),
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(grid, FrameworkElement.HeightProperty, binding);
            var model = new DocumentViewModel();
            DocumentViewModelsTabs.Add(new TabItem()
            {
                Content = grid,
                DataContext = model
            });
            DocumentViewModels.Add(model);
            _selectedDocumentIndex = DocumentViewModelsTabs.Count - 1;
            OnPropertyChanged("SelectedDocumentIndex");
        }

        public int SelectedDocumentIndex
        {
            get { return _selectedDocumentIndex; }
            set
            {
                _selectedDocumentIndex = value;
                OnPropertyChanged("SelectedDocumentIndex");

                var size =
                    ((WindowsFormsHost) ((Grid) DocumentViewModelsTabs[SelectedDocumentIndex].Content).Children[0])
                        .Child.Size;
                Controller.activateDocement(Session, DocumentViewModels[SelectedDocumentIndex].DocumentID, size.Width,
                    size.Height);
            }
        }

        public ObservableCollection<TabItem> DocumentViewModelsTabs
        {
            get { return _documentViewModelsTabs; }
            set
            {
                _documentViewModelsTabs = value; 
                OnPropertyChanged("DocumentViewModels");
            }
        }

        public List<DocumentViewModel> DocumentViewModels
        {
            get { return _documentViewModels; }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                OnPropertyChanged("WindowWidth");
            }
        }

        public double WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                _windowHeight = value;
                OnPropertyChanged("WindowHeight");
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
        private ObservableCollection<TabItem> _documentViewModelsTabs = new ObservableCollection<TabItem>();
        private int _selectedDocumentIndex;
        private DispatcherTimer _timer;
        private double _windowWidth;
        private double _windowHeight;
        private readonly List<DocumentViewModel> _documentViewModels = new List<DocumentViewModel>();

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

        private void RenderPanelOnLoaded(IntPtr hwnd, int w, int h)
        {
            var activeDocument = Controller.initDocument(Session, hwnd);
            Controller.activateDocement(Session, activeDocument, w, h);
            DocumentViewModels[SelectedDocumentIndex].Title = "Document # " + activeDocument;
            DocumentViewModels[SelectedDocumentIndex].DocumentID = activeDocument;
            IsActive = true;
        }

        private void RenderPanelOnResize(int w, int h)
        {
            Controller.resizeDocument(Session, DocumentViewModels[SelectedDocumentIndex].DocumentID, w, h);
        }

#if OLDDOTNET
        private void ProcessControllerWork(object obj)
#else
        private async void ProcessControllerWork(object obj)
#endif
        {
            IsActive = false;
            ApplicationController.operations type = (ApplicationController.operations) obj;

            object [] data= new object[0];

            Window dialog = null;

            switch (type)
            {
                case ApplicationController.operations.OpPointCreate:
                    dialog = new OnePointDialog();
                    dialog.Title = "Create Point";
                    break;
                case ApplicationController.operations.OpLineCreate:
                    dialog = new TwoPointDialog();
                    dialog.Title = "Create Line";
                    break;
                case ApplicationController.operations.OpCircleCreate:
                    dialog = new TwoPointDialog();
                    dialog.Title = "Create Circle";
                    break;
                case ApplicationController.operations.OpContourCreate:
                    dialog = new ElementIdInputDialog();
                    dialog.Title = "Create Contour";
                    break;
                case ApplicationController.operations.OpDeleteObject:
                    dialog = new ElementIdInputDialog();
                    dialog.Title = "Create Contour";
                    break;
                case ApplicationController.operations.OpDestroyContour:
                    dialog = new ElementIdInputDialog();
                    dialog.Title = "Create Contour";
                    break;
                case ApplicationController.operations.OpSetBackgroundColor:
                    dialog = new ElementIdInputDialog();
                    dialog.Title = "Set color number";
                    break;
                case ApplicationController.operations.OpSetLayersToShow:
                    dialog = new ElementIdInputDialog();
                    dialog.Title = "Set layer number";
                    break;
            }

            try
            {
                bool start = true;

                if (dialog != null)
                {
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog is IDataDialog)
                            data = ((IDataDialog) dialog).Data.ToArray();
                    }
                    else
                        start = false;
                }

#if OLDDOTNET
                if (start)
                    Controller.procOperation(Session, DocumentViewModels[SelectedDocumentIndex].DocumentID,
                        (ApplicationController.operations) obj, data);
#else
                if (start)
                    await Task.Run(delegate
                    {
                        Controller.procOperation(Session, DocumentViewModels[SelectedDocumentIndex].DocumentID,
                            (ApplicationController.operations) obj, data);
                    });
#endif
            }
            catch (Exception e)
            {
                if (dialog != null && dialog.IsVisible)
                    dialog.Close();
                MessageBox.Show("Exception: " + e.Message);
            }
            finally
            {
                IsActive = true;
            }
        }

#endregion

#region Commands

        public RelayCommand DocumentWorkCommand
        {
            get
            {
                return _documentWorkCommand != null
                    ? _documentWorkCommand
                    : (_documentWorkCommand = new RelayCommand(ProcessDocumentWork,
                        o => (DocumentViewModels.Count > 0 && IsActive) || DocumentViewModels.Count == 0));
            }
        }

        public RelayCommand ControllerWorkCommand
        {
            get
            {
                return _controllerWorkCommand != null
                    ? _controllerWorkCommand
                    : (_controllerWorkCommand = new RelayCommand(ProcessControllerWork));
            }
        }

#endregion
    }
}
