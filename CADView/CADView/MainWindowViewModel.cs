using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CADController;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CADView.Dialogs;

namespace CADView
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public

        public MainWindowViewModel()
        {
            Controller = new ApplicationController();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        }

        public void CreateDocument()
        {
            _activeDocument = Controller.initDocument(Session);
            _documents.Add(_activeDocument);
            IsActive = true;
        }

        #endregion

        #region Protected

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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

        private ApplicationController Controller { get; set; }

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

        private async void ProcessControllerWork(object obj)
        {
            IsActive = false;
            ApplicationController.operations type = (ApplicationController.operations) obj;

            object [] data= new object[0];

            IDataDialog dialog = null;

            switch (type)
            {
                case ApplicationController.operations.OpPointCreate:
                    dialog = new OnePointDialog();
                    ((Window) dialog).Title = "Create Point";
                    break;
                case ApplicationController.operations.OpLineCreate:
                    dialog = new TwoPointDialog();
                    ((Window)dialog).Title = "Create Line";
                    break;
                case ApplicationController.operations.OpCircleCreate:
                    dialog = new TwoPointDialog();
                    ((Window)dialog).Title = "Create Circle";
                    break;
                case ApplicationController.operations.OpContourCreate:
                    break;
            }

            try
            {
                bool start = true;

                if (dialog != null)
                {
                    if (((Window) dialog).ShowDialog() == true)
                    {
                        data = dialog.Data.ToArray();
                    }
                    else
                        start = false;
                }

                if (start)
                    await Task.Run(delegate
                    {
                        Controller.procOperation(Session, _activeDocument, (ApplicationController.operations) obj, data);
                    });
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message);
            }
            finally
            {
                IsActive = true;
            }
        }

        #endregion

        #region Commands

        public RelayCommand DocumentWorkCommand => _documentWorkCommand ?? (_documentWorkCommand = new RelayCommand(ProcessDocumentWork,
            o => (_documents.Count > 0 && IsActive) || _documents.Count == 0));
        public RelayCommand ControllerWorkCommand => _controllerWorkCommand ?? (_controllerWorkCommand = new RelayCommand(ProcessControllerWork));

        #endregion
    }
}
