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

        #endregion
    }
}
