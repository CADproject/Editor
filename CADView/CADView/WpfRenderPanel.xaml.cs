using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;

namespace CADView
{
    /// <summary>
    /// Логика взаимодействия для WpfRenderPanel.xaml
    /// </summary>
    public partial class WpfRenderPanel: UserControl, IRenderer
    {
        private struct Geometry
        {
            public double[] Points { get; private set; }
            public int[] Color { get; private set; }
            public double Size { get; private set; }

            public Geometry(double[] points, int[] color, double size)
            {
                Points = points;
                Color = color;
                Size = size;
            }
        }

        public WpfRenderPanel()
        {
            InitializeComponent();
            _index = _counter++;
        }
        
        #region Private

        private readonly Axies _axies = new Axies();
        private static int _counter = 0;
        private int _index = 0;
        private bool _disposed = false;
        private readonly List<Geometry> _objects = new List<Geometry>();
        private readonly object _lock = new object();

        private void GLDraw(object sender, OpenGLEventArgs args)
        {
            if (!IsVisible || !IsEnabled) return;

            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;

            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview matrix.
            gl.LoadIdentity();

            //  Move the geometry into a fairly central position.
            gl.Translate(0f, 0.0f, -6.0f);

            _axies.Render(gl, RenderMode.Design);

            lock (_lock)
            {
                foreach (Geometry o in _objects)
                {
                    gl.LineWidth((float)o.Size);
                    gl.Color(o.Color[0] / 255f, o.Color[1] / 255f, o.Color[2] / 255f);

                    gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
                    gl.VertexPointer(3, 0, o.Points);
                    gl.DrawArrays(OpenGL.GL_LINE_STRIP, 0, 2);
                    gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
                }
            }

            //  Flush OpenGL.
            gl.Flush();
        }

        private void GLInitialized(object sender, OpenGLEventArgs args)
        {
            InitEvents();
        }

        #endregion

        #region Events

        private void InitEvents()
        {
            Renderer.Loaded += RenderOnCreated;
            Renderer.SizeChanged += RenderOnChanged;
            Renderer.MouseWheel += RenderOnWheel;
            Renderer.MouseDown += RenderOnDown;
            Renderer.MouseUp += RenderOnUp;
            Renderer.MouseDoubleClick += RenderOnDoubleClick;
            Renderer.MouseMove += RenderOnMouseMove;
        }

        private void ReleaseEvents()
        {
            Renderer.Loaded -= RenderOnCreated;
            Renderer.SizeChanged -= RenderOnChanged;
            Renderer.MouseWheel -= RenderOnWheel;
            Renderer.MouseDown -= RenderOnDown;
            Renderer.MouseUp -= RenderOnUp;
            Renderer.MouseDoubleClick -= RenderOnDoubleClick;
            Renderer.MouseMove -= RenderOnMouseMove;
        }

        private void RenderOnMouseMove(object sender, MouseEventArgs args)
        {
            var p = args.MouseDevice.GetPosition(this);
            MouseFired?.Invoke(new MouseEventArgsExtended(MouseEventArgsExtended.MouseButtons.Empty, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, 0));
        }

        private void RenderOnDoubleClick(object sender, MouseEventArgs args)
        {
            var p = args.MouseDevice.GetPosition(this);
            MouseEventArgsExtended.MouseButtons b = MouseEventArgsExtended.MouseButtons.Empty;
            if(((MouseButtonEventArgs)args).ChangedButton == MouseButton.Left)
                b = MouseEventArgsExtended.MouseButtons.Left;
            MouseFired?.Invoke(new MouseEventArgsExtended(b, MouseEventArgsExtended.PressedState.Released, true, p.X, p.Y, 0));
        }

        private void RenderOnUp(object sender, MouseEventArgs args)
        {
            var p = args.MouseDevice.GetPosition(this);
            MouseEventArgsExtended.MouseButtons b = MouseEventArgsExtended.MouseButtons.Empty;
            switch (((MouseButtonEventArgs)args).ChangedButton)
            {
                case MouseButton.Left:
                    b = MouseEventArgsExtended.MouseButtons.Left;
                    break;
                case MouseButton.Middle:
                    b = MouseEventArgsExtended.MouseButtons.Middle;
                    break;
                case MouseButton.Right:
                    b = MouseEventArgsExtended.MouseButtons.Right;
                    break;
                case MouseButton.XButton1:
                    break;
                case MouseButton.XButton2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            MouseFired?.Invoke(new MouseEventArgsExtended(b, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, 0));
        }

        private void RenderOnDown(object sender, MouseEventArgs args)
        {
            var p = args.MouseDevice.GetPosition(this);
            MouseEventArgsExtended.MouseButtons b = MouseEventArgsExtended.MouseButtons.Empty;
            switch (((MouseButtonEventArgs)args).ChangedButton)
            {
                case MouseButton.Left:
                    b = MouseEventArgsExtended.MouseButtons.Left;
                    break;
                case MouseButton.Middle:
                    b = MouseEventArgsExtended.MouseButtons.Middle;
                    break;
                case MouseButton.Right:
                    b = MouseEventArgsExtended.MouseButtons.Right;
                    break;
                case MouseButton.XButton1:
                    break;
                case MouseButton.XButton2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            MouseFired?.Invoke(new MouseEventArgsExtended(b, MouseEventArgsExtended.PressedState.Pressed, false, p.X, p.Y, 0));
        }

        private void RenderOnClick(object sender, MouseEventArgs args)
        {
            var p = args.MouseDevice.GetPosition(this);
            MouseFired?.Invoke(new MouseEventArgsExtended(MouseEventArgsExtended.MouseButtons.Empty, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, 0));
        }

        private void RenderOnWheel(object sender, MouseEventArgs args)
        {
            var delta = ((MouseWheelEventArgs) args).Delta;
            var p = args.MouseDevice.GetPosition(this);
            MouseFired?.Invoke(new MouseEventArgsExtended(MouseEventArgsExtended.MouseButtons.Empty, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, delta));
        }

        private void RenderOnChanged(object sender, EventArgs args)
        {
            Resized?.Invoke((int)Renderer.Width, (int)Renderer.Height);
        }

        private void RenderOnCreated(object sender, EventArgs args)
        {
            Created?.Invoke(this);
        }

        public delegate void CreatedEventDelegate(IRenderer sender);
        public static event CreatedEventDelegate Created;

        public delegate void ResizedEventDelegate(int w, int h);
        public static event ResizedEventDelegate Resized;

        public delegate void RenderEventDelegate();
        public static event RenderEventDelegate Rendered;

        public delegate void MouseEventDelegate(MouseEventArgsExtended args);
        public static event MouseEventDelegate MouseFired;

        #endregion

        #region Public

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            ReleaseEvents();
            GC.SuppressFinalize(this);
        }

        public void UpdateGeometry(double[] points, int[] color, double size, bool clearPrevious)
        {
            lock (_lock)
            {
                if (clearPrevious)
                    _objects.Clear();
                _objects.Add(new Geometry(points, color, size));
            }
        }

        #endregion
    }
}
