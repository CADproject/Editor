using System;
using System.ComponentModel;
using CADController;
using System.Collections.Generic;
using System.Windows;
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
        }

        ~MainWindowViewModel()
        {
            foreach (uint d in _documents)
                Controller.finalDocument(Session, d);
            Controller.finalSession(Session);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal ApplicationController Controller { get; set; }

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
        }

        public void CreateDocument()
        {
            _activeDocument = Controller.initDocument(Session);
            _documents.Add(_activeDocument);
            IsActive = true;
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
        private uint _activeDocument;
        readonly List<uint> _documents = new List<uint>();
        private bool _isActive;
        private RelayCommand _documentWorkCommand;
        private RelayCommand _controllerWorkCommand;

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
                        if(dialog is IDataDialog)
                        data = ((IDataDialog)dialog).Data.ToArray();
                    }
                    else
                        start = false;
                }

#if OLDDOTNET
                if (start)
                    Controller.procOperation(Session, _activeDocument, (ApplicationController.operations)obj, data);
#else
                if (start)
                    await Task.Run(delegate
                    {
                        Controller.procOperation(Session, _activeDocument, (ApplicationController.operations)obj, data);
                    });
#endif
            }
            catch (Exception e)
            {
                if(dialog != null && dialog.IsVisible)
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
                        o => (_documents.Count > 0 && IsActive) || _documents.Count == 0));
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
