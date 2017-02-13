using System;
using System.Windows.Forms;

namespace CADView
{
    public partial class RenderPanel : UserControl
    {
        public RenderPanel()
        {
            InitializeComponent();

            this.Load += (sender, args) => { if (Loaded != null) Loaded?.Invoke(this.Handle, this.Width, this.Height); };
            this.SizeChanged+= (sender, args) => { if (Resized != null) Resized(this.Width, this.Height); };
        }

        public delegate void LoadedEventDelegate(IntPtr hwnd, int w, int h);
        public static event LoadedEventDelegate Loaded;

        public delegate void ResizedEventDelegate(int w, int h);
        public static event ResizedEventDelegate Resized;

    }
}
