using System;
using System.Windows.Forms;

namespace CADView
{
    public partial class RenderPanel : UserControl
    {
        public RenderPanel()
        {
            InitializeComponent();

            HandleCreated += (sender, args) => { if (Loaded != null) Loaded?.Invoke(Handle, Width, Height); };
            SizeChanged += (sender, args) => { if (Resized != null) Resized(Width, Height); };
            Paint += (sender, args) => { if (Rendered != null) Rendered(); };
            MouseWheel += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            MouseClick += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            MouseDown += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            MouseUp += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            MouseDoubleClick += (sender, args) => { if (MouseFired != null) MouseFired(args); };
            MouseMove += (sender, args) => { if (MouseFired != null) MouseFired(args); };

            DoubleBuffered = true;
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
