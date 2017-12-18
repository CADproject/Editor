using System;
using System.Windows.Forms;
using CADController;

namespace CADView
{
    public sealed partial class RenderPanel : UserControl
    {

        public DocumentModel Model { get; set; }

        public RenderPanel(DocumentModel model)
        {
            Model = model;
            InitializeComponent();

            //HandleCreated += RenderOnCreated;
            //SizeChanged += RenderOnChanged;
            //Paint += RenderOnPaintEventHandler;
            //MouseWheel += RenderOnWheel;
            //MouseClick += RenderOnClick;
            //MouseDown += RenderOnDown;
            //MouseUp += RenderOnUp;
            //MouseDoubleClick += RenderOnDoubleClick;
            //MouseMove += RenderOnMouseMove;

            this.Dispose(true);
            DoubleBuffered = true;
        }
        /*
        private void RenderOnMouseMove(object sender, MouseEventArgs args)
        {
            if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args));
        }

        private void RenderOnDoubleClick(object sender, MouseEventArgs args)
        {
            if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args));
        }

        private void RenderOnUp(object sender, MouseEventArgs args)
        {
            if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args, MouseEventArgsExtended.PressedState.Released));
        }

        private void RenderOnDown(object sender, MouseEventArgs args)
        {
            if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args));
        }

        private void RenderOnClick(object sender, MouseEventArgs args)
        {
            if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args));
        }

        private void RenderOnWheel(object sender, MouseEventArgs args)
        {
            if (MouseFired != null) MouseFired(new MouseEventArgsExtended(args));
        }

        private void RenderOnPaintEventHandler(object sender, PaintEventArgs args)
        {
            if (Rendered != null) Rendered();
        }

        private void RenderOnChanged(object sender, EventArgs args)
        {
            if (Resized != null) Resized(Width, Height);
        }

        private void RenderOnCreated(object sender, EventArgs args)
        {
            if (Loaded != null) Loaded?.Invoke(Handle, Width, Height, Model);
        }

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            HandleCreated -= RenderOnCreated;
            SizeChanged -= RenderOnChanged;
            Paint -= RenderOnPaintEventHandler;
            MouseWheel -= RenderOnWheel;
            MouseClick -= RenderOnClick;
            MouseDown -= RenderOnDown;
            MouseUp -= RenderOnUp;
            MouseDoubleClick -= RenderOnDoubleClick;
            MouseMove -= RenderOnMouseMove;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        */
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
