using System;
using System.Windows.Forms;
using CADController;

namespace CADView
{
    public sealed partial class RenderPanel : UserControl
    {
        public class MouseEventArgsExtended : MouseEventArgs
        {
            public enum PressedState
            {
                Pressed,
                Released,
            }

            public PressedState State { get; private set; }

            public MouseEventArgsExtended(MouseButtons button, int clicks, int x, int y, int delta,
                PressedState state = PressedState.Pressed)
                : base(button, clicks, x, y, delta)
            {
                State = state;
            }

            public MouseEventArgsExtended(MouseEventArgs args, PressedState state = PressedState.Pressed)
                : base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
            {
                State = state;
            }
        }

        public DocumentModel Model { get; set; }

        public RenderPanel(DocumentModel model)
        {
            Model = model;
            InitializeComponent();

            HandleCreated += (sender, args) => { if (Loaded != null) Loaded?.Invoke(Handle, Width, Height, Model); };
            SizeChanged += (sender, args) => { if (Resized != null) Resized(Width, Height); };
            Paint += (sender, args) => { if (Rendered != null) Rendered(); };
            MouseWheel += (sender, args) => { if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args)); };
            MouseClick += (sender, args) => { if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args)); };
            MouseDown += (sender, args) => { if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args)); };
            MouseUp += (sender, args) => { if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args, MouseEventArgsExtended.PressedState.Released)); };
            MouseDoubleClick += (sender, args) => { if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args)); };
            MouseMove += (sender, args) => { if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args)); };

            DoubleBuffered = true;
        }

        public delegate void LoadedEventDelegate(IntPtr hwnd, int w, int h, Document model);
        public static event LoadedEventDelegate Loaded;

        public delegate void ResizedEventDelegate(int w, int h);
        public static event ResizedEventDelegate Resized;

        public delegate void RenderEventDelegate();
        public static event RenderEventDelegate Rendered;

        public delegate void MouseEventDelegate(MouseEventArgsExtended args);
        public static event MouseEventDelegate MouseFired;
    }
}
