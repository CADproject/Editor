using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CADController;

namespace CADView
{
    public class DocumentModel : Document, INotifyPropertyChanged
    {
        #region Public

        public DocumentModel(Layer firstLayer) : base(firstLayer)
        {

        }

        public DocumentModel(IEnumerable<Layer> layers) : base(layers)
        {

        }

        public DocumentModel(Document document) : base(document)
        {

        }

        public override string Title
        {
            get { return base.Title; }
            set
            {
                base.Title = value;
                OnPropertyChanged();
            }
        }

        public override uint DocumentID
        {
            get { return base.DocumentID; }
            set
            {
                base.DocumentID = value;
                OnPropertyChanged();
            }
        }

        public override Layer ActiveLayer
        {
            get { return base.ActiveLayer; }
            set
            {
                base.ActiveLayer = value;
                OnPropertyChanged();
            }
        }

        public IRenderer Renderer { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private

        #endregion
    }
}
