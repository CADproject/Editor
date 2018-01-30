using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SessionId = System.UInt32;
using DocumentId = System.UInt32;
using OperationId = System.UInt32;
using ObjectId = System.UInt32;

namespace CADController
{
    abstract class OperationController: IController
    {
        private bool _inited = false;
        protected Action FinilizeOperation { get; private set; }
        public IntPtr CurrentSession { get; set; } = IntPtr.Zero;
        public DocumentId CurrentDocument { get; set; } = 0;
        public double X { get; set; }
        public double Y { get; set; }

        public virtual void Init(Action finilizeOperation, IntPtr currentSession, DocumentId currentDocument)
        {
            FinilizeOperation = finilizeOperation;
            CurrentSession = currentSession;
            CurrentDocument = currentDocument;
            _inited = true;
        }

        public virtual void InputEvent(int evId)
        {
            if(!_inited)
                throw new Exception("Operation did not inited.");
        }

        public virtual void MouseMove(double x, double y)
        {
            if (!_inited)
                throw new Exception("Operation did not inited.");
            X = x;
            Y = y;
        }

        public virtual void SendInt(int value)
        {
            if (!_inited)
                throw new Exception("Operation did not inited.");
            throw new NotImplementedException();
        }

        public virtual void SendDouble(double value)
        {
            if (!_inited)
                throw new Exception("Operation did not inited.");
            throw new NotImplementedException();
        }

        public virtual void SendString(string value)
        {
            if (!_inited)
                throw new Exception("Operation did not inited.");
            throw new NotImplementedException();
        }

        public virtual void CancelOperation()
        {
            if (!_inited)
                throw new Exception("Operation did not inited.");
            throw new NotImplementedException();
        }
    }

    class OpLineCreate : OperationController
    {
        IntPtr start = IntPtr.Zero;
        IntPtr end = IntPtr.Zero;
        ObjectId lineId;
        IntPtr newLineGen;

        public override void InputEvent(int evId)
        {
            base.InputEvent(evId);
            if(evId != 4) return;
            if (start == IntPtr.Zero)
            {
                start = CoreWrapper.nodeFactory(X, Y);
            }
            if (end == IntPtr.Zero)
            {
                end = CoreWrapper.nodeFactory(X, Y);
            }
            else
            {
                end = CoreWrapper.nodeFactory(X, Y);

                if (newLineGen != IntPtr.Zero)
                    CoreWrapper.detachFromBase(CurrentSession, CurrentDocument, lineId);

                newLineGen = CoreWrapper.genericFactory(CoreWrapper.lineFactory(start, end), (uint)0);
                lineId = CoreWrapper.attachToBase(CurrentSession, CurrentDocument, newLineGen);

                CoreWrapper.commit(CurrentSession, CurrentDocument);
                FinilizeOperation();
            }
        }

        public override void MouseMove(double x, double y)
        {
            base.MouseMove(x, y);
            if (start != IntPtr.Zero)
            {
                //CoreWrapper.Destroy(end)
                end = CoreWrapper.nodeFactory(X, Y);

                if (newLineGen != IntPtr.Zero)
                    CoreWrapper.detachFromBase(CurrentSession, CurrentDocument, lineId);

                newLineGen = CoreWrapper.genericFactory(CoreWrapper.lineFactory(start, end), (uint)0);
                lineId = CoreWrapper.attachToBase(CurrentSession, CurrentDocument, newLineGen);
            }
        }

        public override void SendDouble(double value)
        {
        }

        public override void CancelOperation()
        {
            if (newLineGen != IntPtr.Zero)
                CoreWrapper.detachFromBase(CurrentSession, CurrentDocument, lineId);
        }
    }

    public static class ControllerFactory
    {
        public static IApplicationController CreateController()
        {
            Console.WriteLine("Creating controller...");
            var ctrl = new ApplicationController();
            return ctrl;
        }
    }

    internal class ApplicationController: FakeApplicationController
    {
        private OperationController _currentOperation;

        public IntPtr CurrentSession { get; set; }
        public DocumentId CurrentDocument { get; set; }

        public override unsafe Status OpenSession(Dictionary<string, Callback> delegates)
        {
            int count = delegates.Count;
            string [] names = new string[count];
            IntPtr []functions = new IntPtr[count];
            int index = 0;
            foreach (var pair in delegates)
            {
                names[index] = pair.Key;
                functions[index] = Marshal.GetFunctionPointerForDelegate(pair.Value);
                index++;
            }
            CurrentSession = CoreWrapper.sessionFactory(functions, names, count);

            return base.OpenSession(delegates);
        }

        public override Status Operation(int opId)
        {
            if (_currentOperation != null)
            {
                _currentOperation.CancelOperation();
                //corewrapper.clean(...);
            }
            switch ((UniversalOperations)opId)
            {
                case UniversalOperations.NewDocument:
                    IntPtr document = CoreWrapper.documentFactory(IntPtr.Zero);
                    CurrentDocument = CoreWrapper.attachDocument(CurrentSession, document);
                    UnmanagedArray<int> udata = new UnmanagedArray<int>(new[] {0, 1});
                    CoreWrapper.setLayers(CurrentSession, CurrentDocument, udata, udata.Size);
                    break;
                case UniversalOperations.OpenDocument:
                    break;
                case UniversalOperations.SaveDocument:
                    break;
                case UniversalOperations.rename_document:
                    break;
                case UniversalOperations.copy_document:
                    break;
                case UniversalOperations.close_document:
                    break;
                case UniversalOperations.close_all_docs:
                    break;
                case UniversalOperations.StepBackward:
                    break;
                case UniversalOperations.StepForward:
                    break;
                case UniversalOperations.Pen:
                    break;
                case UniversalOperations.Line1:
                    _currentOperation = new OpLineCreate();
                    break;
                case UniversalOperations.Line2:
                    break;
                case UniversalOperations.Line3:
                    break;
                case UniversalOperations.Line4:
                    break;
                case UniversalOperations.Arc1:
                    break;
                case UniversalOperations.Arc2:
                    break;
                case UniversalOperations.Circle1:
                    break;
                case UniversalOperations.Circle2:
                    break;
                case UniversalOperations.BrokenLine:
                    break;
                case UniversalOperations.Spline:
                    break;
                case UniversalOperations.Eraser:
                    break;
                case UniversalOperations.Trimming:
                    break;
                case UniversalOperations.EnlargeElement:
                    break;
                case UniversalOperations.LinkLines:
                    break;
                case UniversalOperations.DestroyLine:
                    break;
                case UniversalOperations.Correct:
                    break;
                case UniversalOperations.CreateNode:
                    break;
                case UniversalOperations.DeleteNode:
                    break;
                case UniversalOperations.Measure:
                    break;
                case UniversalOperations.Protractor:
                    break;
                case UniversalOperations.copy_object:
                    break;
                case UniversalOperations.cut_object:
                    break;
                case UniversalOperations.paste_object:
                    break;
                case UniversalOperations.move_object:
                    break;
                case UniversalOperations.rotate_object:
                    break;
                case UniversalOperations.display_in_circle_object:
                    break;
                case UniversalOperations.display_symmetyrically_object:
                    break;
                case UniversalOperations.scale_object:
                    break;
                case UniversalOperations.MoveView:
                    break;
                case UniversalOperations.EnlargeView:
                    break;
                case UniversalOperations.DiminishView:
                    break;
                case UniversalOperations.ShowAll:
                    break;
                case UniversalOperations.AddLayer:
                    break;
                case UniversalOperations.rename_layer:
                    break;
                case UniversalOperations.DeleteLayer:
                    break;
                case UniversalOperations.set_active_layer:
                    break;
                case UniversalOperations.set_visible_layers:
                    break;
                case UniversalOperations.set_invisible_layers:
                    break;
                case UniversalOperations.Count:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(opId), opId, null);
            }
            if (_currentOperation != null)
            {
                _currentOperation.Init(() => _currentOperation = null, CurrentSession, CurrentDocument);
            }
            return base.Operation(opId);
        }

        public override void InputEvent(int evId)
        {
            base.InputEvent(evId);
            _currentOperation?.InputEvent(evId);
        }

        public override void MouseMove(double x, double y)
        {
            base.MouseMove(x, y);
            _currentOperation?.MouseMove(x, y);
        }

        public override void SendDouble(double value)
        {
            base.SendDouble(value);
            _currentOperation?.SendDouble(value);
        }
    }

    internal class FakeApplicationController : BaseApplicationController
    {
        private Callback _drawCallback;
        private Dictionary<string, Callback> testArray;

        public override Status OpenSession(Dictionary<string, Callback> delegates)
        {
            testArray = delegates;
            delegates.TryGetValue(nameof(IViewCallback.DrawGeometry), out _drawCallback);
            return base.OpenSession(delegates);
        }

        public override void InputEvent(int evId)
        {
            base.InputEvent(evId);
            if (evId == 6)
            {
                IntPtr[] functions = new IntPtr[testArray.Count];
                int index = 0;
                foreach (var pair in testArray)
                {
                    functions[index] = Marshal.GetFunctionPointerForDelegate(pair.Value);
                    index++;
                }

                CoreWrapper.TestPInvoke(_drawCallback, functions);
            }
        }
    }

    internal abstract class BaseApplicationController: IApplicationController
    {
        protected Callback _logCallback;
        /// <summary>
        /// to save delegates from garbage collector.
        /// </summary>
        private Dictionary<string, Callback> _delegates;

        protected BaseApplicationController()
        {
            Console.WriteLine(GetCurrentMethod());
        }

        protected virtual string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase method = sf.GetMethod();
            return method.ReflectedType?.Name + "::" + method.Name;
        }

        public virtual Status OpenSession(Dictionary<string, Callback> delegates)
        {
            if (delegates == null) throw new ArgumentNullException(nameof(delegates));
            _delegates = delegates;
            if (delegates != null && delegates.ContainsKey(nameof(IViewCallback.ConsoleLog)))
                _logCallback = delegates[nameof(IViewCallback.ConsoleLog)];
            _logCallback?.Invoke(new CallbackValues {line = GetCurrentMethod()});
            _logCallback?.Invoke(new CallbackValues
            {
                line = delegates.Aggregate("", (s1, s2) => s1 + "/" + s2.Key)
            });
            return Status.Success;
        }

        public virtual Status CloseSession()
        {
            _logCallback?.Invoke(new CallbackValues { line = GetCurrentMethod() });
            Console.WriteLine(GetCurrentMethod());
            return Status.Success;
        }

        public virtual void SetActiveDocument(uint docId)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {docId}" });
            Console.WriteLine($"{GetCurrentMethod()}, {docId}");
        }

        public virtual void InputEvent(int evId)
        {
            _logCallback?.Invoke(new CallbackValues {line = $"{GetCurrentMethod()}, {evId}"});
            Console.WriteLine($"{GetCurrentMethod()}, {evId}");
        }

        public virtual void MouseMove(double x, double y)
        {
            return;
            _logCallback?.Invoke(new CallbackValues { line = GetCurrentMethod() });
            Console.WriteLine(GetCurrentMethod());
        }

        public virtual Status Operation(int opId)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {opId}" });
            Console.WriteLine($"{GetCurrentMethod()}, {opId}");
            return Status.Success;
        }

        public virtual void SendInt(int value)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {value}" });
            Console.WriteLine($"{GetCurrentMethod()}, {value}");
        }

        public virtual void SendDouble(double value)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {value}" });
            Console.WriteLine($"{GetCurrentMethod()}, {value}");
        }

        public virtual void SendString(string value)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {value}" });
            Console.WriteLine($"{GetCurrentMethod()}, {value}");
        }
    }
}