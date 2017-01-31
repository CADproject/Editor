using System;
using System.Runtime.InteropServices;

namespace CADController
{
    using ObjectId = System.UInt32;
    using DocumentId = System.UInt32;

    enum Color { black, red, green, blue, yellow };
    enum Thickness { one, two, three, four, five };
    enum dataType { unsigned, intptr };

    internal unsafe struct CoreWrapper
    {
        const ObjectId not_from_base = 0;

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sessionFactory();

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern DocumentId attachDocument(IntPtr pObject, IntPtr doc);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ObjectId attachToBase(IntPtr pObject, DocumentId docID, IntPtr genObj);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr detachFromBase(IntPtr pObject, DocumentId docID, ObjectId objID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void attachToBuffer(IntPtr pObject, DocumentId docID, IntPtr genObj);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getGenTopology(IntPtr pObject, DocumentId docID, ObjectId objID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint getGenLayer(IntPtr pObject, DocumentId docID, ObjectId objID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLayers(IntPtr pObject, DocumentId docID, IntPtr newLayers);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBackgroundColor(IntPtr pObject, DocumentId docID, Color color);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void toScreen(IntPtr pObject, DocumentId docID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void commit(IntPtr pObject, DocumentId docID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void undo(IntPtr pObject, DocumentId docID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void redo(IntPtr pObject, DocumentId docID);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr documentFactory();

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr nodeFactory(double x, double y);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pointFactory(IntPtr node);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lineFactory(IntPtr start, IntPtr end);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr circleFactory(IntPtr center, IntPtr side);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr contourFactory(IntPtr edges, uint size);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getContourEdges(IntPtr pObject, IntPtr pEdges, ref uint size);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr genericFactory(IntPtr primitive, uint layer = 0,
            Color color = Color.black, Thickness thickness = Thickness.three);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint getGenericLayer(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
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