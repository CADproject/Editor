using System;
using System.Windows.Forms;

namespace CADView
{
    public partial class RenderPanel : UserControl
    {
        public RenderPanel()
        {
            InitializeComponent();

            this.HandleCreated += (sender, args) => { if (Loaded != null) Loaded?.Invoke(this.Handle, this.Width, this.Height); };
            this.SizeChanged += (sender, args) => { if (Resized != null) Resized(this.Width, this.Height); };
            this.Paint += (sender, args) => { if (Rendered != null) Rendered(); };
            this.MouseWheel += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            this.MouseClick += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            this.MouseDown += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            this.MouseUp += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            this.MouseDoubleClick += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            this.MouseMove += (sender, args) => { if (MouseFired != null) MouseFired(args); };

            this.DoubleBuffered = true;
        }

        public delegate void LoadedEventDelegate(IntPtr hwnd, int w, int h);
        public static event LoadedEventDelegate Loaded;

        public delegate void ResizedEventDelegate(int w, int h);
        public static event ResizedEventDelegate Resized;

        public delegate void RenderEventDelegate();
        public static event RenderEventDelegate Rendered;

        public delegate void MouseEventDelegate(MouseEventArgs args);
        public static event MouseEventDelegate MouseFired;
    }
}
