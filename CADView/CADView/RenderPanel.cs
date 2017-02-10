using System;
using System.Windows.Forms;

namespace CADView
{
    public partial class RenderPanel : UserControl
    {
        public RenderPanel()
        {
            InitializeComponent();

            this.Load+= (sender, args) => Loaded?.Invoke(this.Handle);
        }

        public delegate void LoadedEventDelegate(IntPtr hwnd);
        public static event LoadedEventDelegate Loaded;
    }
}
