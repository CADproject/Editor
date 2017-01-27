using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CADController
{
    using ObjectId = System.UInt32;
    using DocumentId = System.UInt32;

    enum COLOR { BLACK, RED, GREEN, BLUE, YELLOW };
    enum THICKNESS { ONE, TWO, THREE, FOUR, FIVE };
    enum dataType { unsigned, intptr };

    class CoreWrapper
    {
        const ObjectId NOT_FROM_BASE = 0;

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
        public static extern void setBackgroundColor(IntPtr pObject, DocumentId docID, COLOR color);

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
        public static extern IntPtr contourFactory(IntPtr edges);
        
        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getContourEdges(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr genericFactory(IntPtr primitive, uint layer = 0,
            COLOR color = COLOR.BLACK, THICKNESS thickness = THICKNESS.THREE);
        
        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint getGenericLayer(IntPtr pObject);
        
        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getGenericTopology(IntPtr pObject);
    }

    class STLVector
    {
        private IntPtr _pointer;
        private dataType _type;
        
        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr createVectorU();

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void deleteVectorU(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void push_backU(IntPtr pObject, uint value);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pop_backU(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void clearU(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint atU(IntPtr pObject, uint index);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint sizeU(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr createVectorE();

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void deleteVectorE(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void push_backE(IntPtr pObject, IntPtr value);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pop_backE(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void clearE(IntPtr pObject);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr atE(IntPtr pObject, uint index);

        [DllImport("Core.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint sizeE(IntPtr pObject);

        public STLVector(dataType type)
        {
            _type = type;

            if (type == dataType.unsigned)
                _pointer = createVectorU();
            else if (type == dataType.intptr)
                _pointer = createVectorE();
        }

        public STLVector(IntPtr pointer, dataType type)
        {
            _pointer = pointer;
            _type = type;
        }
        
        public IntPtr getPointer() { return _pointer; }

        public dataType getType() { return _type; }
        
        public void deleteVector()
        {
            if (_type == dataType.unsigned)
                deleteVectorU(_pointer);
            else if (_type == dataType.intptr)
                deleteVectorE(_pointer);

            _pointer = IntPtr.Zero;
        }

        public void push_back(uint value) { push_backU(_pointer, value); }
        public void push_back(IntPtr obj) { push_backE(_pointer, obj); }

        public void pop_back()
        {
            if (_type == dataType.unsigned)
                pop_backU(_pointer);
            else if (_type == dataType.intptr)
                pop_backE(_pointer);
        }

        public void clear()
        {
            if (_type == dataType.unsigned)
                clearU(_pointer);
            else if (_type == dataType.intptr)
                clearE(_pointer);
        }

        public uint atU(uint index) { return atU(_pointer, index); }
        public IntPtr atE(uint index) { return atE(_pointer, index); }

        public uint size()
        {
            if (_type == dataType.unsigned)
                return sizeU(_pointer);
            else
                return sizeE(_pointer);
        }
    }
} 