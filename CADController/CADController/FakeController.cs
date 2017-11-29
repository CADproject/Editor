using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CADController
{
    public class FakeController: IController
    {
        private static readonly List<FakeController> _controllers = new List<FakeController>();
        
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

        public Status OpenSession(params Delegate[] delegates)
        {
            Console.WriteLine(GetCurrentMethod());
            return Status.Success;
        }

        public Status CloseSession()
        {
            Console.WriteLine(GetCurrentMethod());
            return Status.Success;
        }

        public void SetActiveDocument(uint docId)
        {
            Console.WriteLine(GetCurrentMethod());
        }

        public void Event(int evId)
        {
            Console.WriteLine(GetCurrentMethod());
        }

        public void MouseMove(double x, double y)
        {
            Console.WriteLine(GetCurrentMethod());
        }

        public Status Operation(int opId)
        {
            Console.WriteLine(GetCurrentMethod());
            return Status.Success;
        }

        public void SendInt(int value)
        {
            Console.WriteLine(GetCurrentMethod());
        }

        public void SendDouble(double value)
        {
            Console.WriteLine(GetCurrentMethod());
        }

        public void SendString(string value)
        {
            Console.WriteLine(GetCurrentMethod());
        }
    }
}