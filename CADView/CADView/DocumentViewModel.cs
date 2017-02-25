using System.ComponentModel;

namespace CADView
{
    public class DocumentViewModel : INotifyPropertyChanged
    {
        #region Public

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public uint DocumentID
        {
            get { return _documentId; }
            set { _documentId = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private

        private string _title;
        private uint _documentId;

        #endregion
    }
}
