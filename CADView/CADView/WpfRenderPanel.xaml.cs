using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SharpGL;
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
            public long Id { get; private set; }
            public double[] Points { get; private set; }
            public int[] Color { get; private set; }
            public double Size { get; private set; }

            public Geometry(long id, double[] points, int[] color, double size)
            {
                Id = id;
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
        private readonly Dictionary<long, Geometry> _objects = new Dictionary<long, Geometry>();
        private readonly object _lock = new object();
        private OpenGL _gl;
        private double _viewHeight = -60;
        private Point _mouse = default(Point);
        private Point? _screenPoint = null;

        static double Dot(double[] a, double[] b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }

        private void GLDraw(object sender, OpenGLEventArgs args)
        {
            if (!IsVisible || !IsEnabled) return;
            
            //  Get the OpenGL instance that's been passed to us.
            _gl = args.OpenGL;

            if (_screenPoint.HasValue)
            {
                double[] coordsA = _gl.UnProject(_screenPoint.Value.X, _screenPoint.Value.Y, 0);
                double[] coordsB = _gl.UnProject(_screenPoint.Value.X, _screenPoint.Value.Y, 1);
                double[] r = new double[3]{coordsB[0]-coordsA[0], coordsB[1] - coordsA[1] , coordsB[2] - coordsA[2] };
                //T = -dot(a - b, n) / dot(v, n)
                double t = -Dot(coordsA, new double[3] {0, 0, 1}) / Dot(r, new double[3] {0, 0, 1});
                //P = a + v * T
                _mouse = new Point(coordsA[0] + r[0]*t, -(coordsA[1] + r[1] * t));
            }

            //  Clear the color and depth buffers.
            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview matrix.
            _gl.LoadIdentity();

            //  Move the geometry into a fairly central position.
            _gl.Translate(0f, 0.0f, _viewHeight);

            _axies.Render(_gl, RenderMode.Design);

            lock (_lock)
            {
                foreach (Geometry o in _objects.Values)
                {
                    _gl.LineWidth((float)o.Size);
                    _gl.Color(o.Color[0] / 255f, o.Color[1] / 255f, o.Color[2] / 255f);

                    _gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
                    _gl.VertexPointer(3, 0, o.Points);
                    _gl.DrawArrays(OpenGL.GL_LINE_STRIP, 0, 2);
                    _gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
                }
            }

            //  Flush OpenGL.
            _gl.Flush();
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
            Renderer.KeyUp += RendererKeyUp;
            Renderer.Focusable = true;
            Renderer.Focus();
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
            Renderer.KeyUp -= RendererKeyUp;
        }

        Point GetMouse(MouseEventArgs args)
        {
            Renderer.Focus();
            _screenPoint = args.MouseDevice.GetPosition(this);
            return _mouse;
        }

        private void RenderOnMouseMove(object sender, MouseEventArgs args)
        {
            Point p = GetMouse(args);
            MouseFired?.Invoke(new MouseEventArgsExtended(MouseEventArgsExtended.MouseButtons.Empty, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, 0));
        }

        private void RenderOnDoubleClick(object sender, MouseEventArgs args)
        {
            Point p = GetMouse(args);
            MouseEventArgsExtended.MouseButtons b = MouseEventArgsExtended.MouseButtons.Empty;
            if(((MouseButtonEventArgs)args).ChangedButton == MouseButton.Left)
                b = MouseEventArgsExtended.MouseButtons.Left;
            MouseFired?.Invoke(new MouseEventArgsExtended(b, MouseEventArgsExtended.PressedState.Released, true, p.X, p.Y, 0));
        }

        private void RenderOnUp(object sender, MouseEventArgs args)
        {
            Point p = GetMouse(args);
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
            Point p = GetMouse(args);
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
            Point p = GetMouse(args);
            MouseFired?.Invoke(new MouseEventArgsExtended(MouseEventArgsExtended.MouseButtons.Empty, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, 0));
        }

        private void RenderOnWheel(object sender, MouseEventArgs args)
        {
            var delta = ((MouseWheelEventArgs) args).Delta;
            _viewHeight += Math.Sign(delta) * 1;
            Point p = GetMouse(args);
            MouseFired?.Invoke(new MouseEventArgsExtended(MouseEventArgsExtended.MouseButtons.Empty, MouseEventArgsExtended.PressedState.Released, false, p.X, p.Y, delta));
        }

        private void RendererKeyUp(object sender, KeyEventArgs e)
        {
            KeyboardFired?.Invoke(e);
        }

        private void RenderOnChanged(object sender, EventArgs args)
        {
            Resized?.Invoke((int)Renderer.ActualHeight, (int)Renderer.ActualHeight);
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

        public delegate void KeyboardEventDelegate(KeyEventArgs args);
        public static event KeyboardEventDelegate KeyboardFired;

        #endregion

        #region Public

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            ReleaseEvents();
            GC.SuppressFinalize(this);
        }

        public void UpdateGeometry(long id, double[] points, int[] color, double size, GeometryAction action)
        {
            lock (_lock)
            {
                switch (action)
                {
                    case GeometryAction.Update:
                        _objects[id] = new Geometry(id, points, color, size);
                        break;
                    case GeometryAction.ClearAll:
                        _objects.Clear();
                        _objects[id] = new Geometry(id, points, color, size);
                        break;
                    case GeometryAction.Remove:
                        if (_objects.ContainsKey(id))
                            _objects.Remove(id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
            }
        }

        #endregion
    }
}
