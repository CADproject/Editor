using System;
using System.Windows.Controls;
using System.Windows.Input;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.WPF;

namespace CADView
{
    /// <summary>
    /// Логика взаимодействия для WpfRenderPanel.xaml
    /// </summary>
    public partial class WpfRenderPanel: UserControl
    {
        public WpfRenderPanel()
        {
            InitializeComponent();
            _index = _counter++;
        }

        private void GLDraw(object sender, OpenGLEventArgs args)
        {
            if(!IsVisible || !IsEnabled) return;

            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;

            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview matrix.
            gl.LoadIdentity();

            //  Move the geometry into a fairly central position.
            gl.Translate(0f, 0.0f, -6.0f);

            _axies.Render(gl, RenderMode.Design);





            //  Flush OpenGL.
            gl.Flush();
        }

        private void GLInitialized(object sender, OpenGLEventArgs args)
        {
            InitEvents();
        }

        private readonly Axies _axies = new Axies();
        private static int _counter = 0;
        private int _index = 0;

        #region Events

        void InitEvents()
        {
            Renderer.Loaded += RenderOnCreated;
            Renderer.SizeChanged += RenderOnChanged;
            Renderer.MouseWheel += RenderOnWheel;
            Renderer.MouseDown += RenderOnDown;
            Renderer.MouseUp += RenderOnUp;
            Renderer.MouseDoubleClick += RenderOnDoubleClick;
            Renderer.MouseMove += RenderOnMouseMove;
        }

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

        private void RenderOnChanged(object sender, EventArgs args)
        {
            if (Resized != null) Resized((int)Renderer.Width, (int)Renderer.Height);
        }

        private void RenderOnCreated(object sender, EventArgs args)
        {
            if (Loaded != null) Loaded?.Invoke(/*Handle, Width, Height, Model*/);
        }

        public delegate void LoadedEventDelegate(/*IntPtr hwnd, int w, int h, Document model*/);
        public static event LoadedEventDelegate Loaded;

        public delegate void ResizedEventDelegate(int w, int h);
        public static event ResizedEventDelegate Resized;

        public delegate void RenderEventDelegate();
        public static event RenderEventDelegate Rendered;

        public delegate void MouseEventDelegate(MouseEventArgsExtended args);
        public static event MouseEventDelegate MouseFired;
        #endregion
    }
}
