using System;
using System.Runtime.InteropServices;

namespace CADController
{
    using SessionId = System.UInt32;
    using DocumentId = System.UInt32;
    using OperationId = System.UInt32;
    using ObjectId = System.UInt32;

    enum Color { black, red, green, blue, yellow };
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
        public static extern void setLayers(IntPtr pObject, DocumentId docID, IntPtr newLayers);

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
    }

    public unsafe class UnmanagedArray
    {
        public IntPtr _pointer;
        public int _size;

        public UnmanagedArray(IntPtr[] array)
        {
            _size = array.Length;
            _pointer = Marshal.AllocHGlobal(_size * sizeof(IntPtr));
            Marshal.Copy(array, 0, _pointer, _size);
        }
        
        ~UnmanagedArray()
        {
            Marshal.FreeHGlobal(_pointer);
        }

        public static implicit operator IntPtr(UnmanagedArray ptr)
        {
            return ptr._pointer;
        }

        public IntPtr getPointer() { return _pointer; }
        public int getSize() { return _size; }
    }
}