using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CADController
{
    public class FakeController: IController
    {
        private static readonly List<FakeController> _controllers = new List<FakeController>();
        private Callback _logCallback;

        public static IController CreateController()
        {
            Console.WriteLine(GetCurrentMethod());
            var ctrl = new FakeController();
            _controllers.Add(ctrl);
            return ctrl;
        }

        private FakeController()
        {
            Console.WriteLine(GetCurrentMethod());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public Status OpenSession(params Callback[] delegates)
        {
            if (delegates.Length > 0)
                _logCallback = delegates[0];
            _logCallback?.Invoke(new CallbackValues {line = GetCurrentMethod()});
            return Status.Success;
        }

        public Status CloseSession()
        {
            _logCallback?.Invoke(new CallbackValues { line = GetCurrentMethod() });
            Console.WriteLine(GetCurrentMethod());
            return Status.Success;
        }

        public void SetActiveDocument(uint docId)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {docId}" });
            Console.WriteLine($"{GetCurrentMethod()}, {docId}");
        }

        public void Event(int evId)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {evId}" });
            Console.WriteLine($"{GetCurrentMethod()}, {evId}");
        }

        public void MouseMove(double x, double y)
        {
            _logCallback?.Invoke(new CallbackValues { line = GetCurrentMethod() });
            Console.WriteLine(GetCurrentMethod());
        }

        public Status Operation(int opId)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {opId}" });
            Console.WriteLine($"{GetCurrentMethod()}, {opId}");
            return Status.Success;
        }

        public void SendInt(int value)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {value}" });
            Console.WriteLine($"{GetCurrentMethod()}, {value}");
        }

        public void SendDouble(double value)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {value}" });
            Console.WriteLine($"{GetCurrentMethod()}, {value}");
        }

        public void SendString(string value)
        {
            _logCallback?.Invoke(new CallbackValues { line = $"{GetCurrentMethod()}, {value}" });
            Console.WriteLine($"{GetCurrentMethod()}, {value}");
        }
    }
}