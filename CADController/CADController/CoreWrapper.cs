using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using SessionId = System.UInt32;
using DocumentId = System.UInt32;
using OperationId = System.UInt32;
using ObjectId = System.UInt32;

namespace CADController
{

    public enum Color
    {
        black = 0,
        red,
        green,
        blue,
        yellow
    };
    enum Thickness { one, two, three, four, five };
    enum dataType { unsigned, intptr };

    internal unsafe struct CoreWrapper
    {
        private const string dllName = "CADCore.dll";
        const ObjectId not_from_base = 0;

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sessionFactory();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern DocumentId attachDocument(IntPtr pObject, IntPtr doc);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr detachDocument(IntPtr pObject, DocumentId docID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroyDocument(IntPtr pObject);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ObjectId attachToBase(IntPtr pObject, DocumentId docID, IntPtr genObj);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr detachFromBase(IntPtr pObject, DocumentId docID, ObjectId objID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void attachToBuffer(IntPtr pObject, DocumentId docID, IntPtr genObj);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getGenTopology(IntPtr pObject, DocumentId docID, ObjectId objID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint getGenLayer(IntPtr pObject, DocumentId docID, ObjectId objID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLayers(IntPtr pObject, DocumentId docID, IntPtr newLayers, uint size);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBackgroundColor(IntPtr pObject, DocumentId docID, Color color);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void commit(IntPtr pObject, DocumentId docID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void undo(IntPtr pObject, DocumentId docID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void redo(IntPtr pObject, DocumentId docID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw(IntPtr pObject, DocumentId docID);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wheel(IntPtr pObject, DocumentId docID, int val);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void activateDocument(IntPtr pObject, DocumentId docID, int w, int h);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void resizeDocument(IntPtr pObject, DocumentId docID, int w, int h);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr documentFactory(IntPtr hwnd);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr nodeFactory(double x, double y);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pointFactory(IntPtr node);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lineFactory(IntPtr start, IntPtr end);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr circleFactory(IntPtr center, IntPtr side);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr contourFactory(IntPtr edges, uint size);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getContourEdges(IntPtr pObject, ref int size);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr genericFactory(IntPtr primitive, uint layer = 0,
            Color color = Color.black, Thickness thickness = Thickness.three);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint getGenericLayer(IntPtr pObject);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getGenericTopology(IntPtr pObject);


        #region test

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TestPInvoke(Callback function);

        #endregion
    }

    public unsafe class UnmanagedArray<T> where T: struct
    {
        private readonly int _size;

        public UnmanagedArray(IEnumerable<T> array)
        {
            if(array == null)
                throw new ArgumentException("Empty array.");
            IEnumerable<T> enumerable = array as T[] ?? array.ToArray();
            _size = enumerable.Count();
            Pointer = Marshal.AllocHGlobal(_size * sizeof (IntPtr));
            if (typeof (T) == typeof (int))
                Marshal.Copy(enumerable.Cast<int>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (byte))
                Marshal.Copy(enumerable.Cast<byte>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (float))
                Marshal.Copy(enumerable.Cast<float>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (IntPtr))
                Marshal.Copy(enumerable.Cast<IntPtr>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (long))
                Marshal.Copy(enumerable.Cast<long>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (short))
                Marshal.Copy(enumerable.Cast<short>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (char))
                Marshal.Copy(enumerable.Cast<char>().ToArray(), 0, Pointer, _size);
            else if (typeof (T) == typeof (double))
                Marshal.Copy(enumerable.Cast<double>().ToArray(), 0, Pointer, _size);
            else
                throw new ArgumentException("Wrong type of array.", typeof (T).FullName);
        }

        ~UnmanagedArray()
        {
            Marshal.FreeHGlobal(Pointer);
        }

        public static implicit operator IntPtr(UnmanagedArray<T> ptr)
        {
            return ptr.Pointer;
        }

        public IntPtr Pointer { get; }

        public uint Size
        {
            get { return (uint) _size; }
        }
    }
}
