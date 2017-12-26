/*This file contains controller sketch.*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace CADController
{
    using SessionId = System.UInt32;
    using DocumentId = System.UInt32;
    using ObjectId = System.UInt32;

    public enum UniversalOperations
    {
        ////операции с документами
        NewDocument = 0,                                        //create_document,
        OpenDocument,                                           //open_document,
        SaveDocument,                                           //save_document,
        rename_document,                                        //rename_document,
        copy_document,                                          //copy_document,
        close_document,                                         //close_document,
        close_all_docs,                                         //close_all_docs,
        StepBackward,                                           //undo,
        StepForward,                                            //redo,
                                                                ////геометрические операции
        Pen,                                                    //draw_curve,
        Line1,                                                  //draw_line,
        Line2,                                                  //draw_line_parallel,
        Line3,                                                  //draw_line_normal,
        Line4,                                                  //draw_line_at_angle,
        Arc1,                                                   //draw_arc_center_two_points,
        Arc2,                                                   //draw_arc_three_points,
        Circle1,                                                //draw_circle_center_point,
        Circle2,                                                //draw_circle_three_points,
        BrokenLine,                                             //draw_polygonal,
        Spline,                                                 //draw_spline,
                                                                ////операции с объектами
        Eraser,                                                 //delete_object,
        Trimming,                                               //trim_object,
        EnlargeElement,                                         //lengthen_object,
        LinkLines,                                              //sew_polygonal,
        DestroyLine,                                            //destroy_polygonal,
        Correct,                                                //correct_object,
        CreateNode,                                             //add_node_to_object,
        DeleteNode,                                             //remove_node_from_object,
        Measure,                                                //measure_distance,
        Protractor,                                             //measure_angle,
        copy_object,                                            //copy_object,
        cut_object,                                             //cut_object,
        paste_object,                                           //paste_object,
        move_object,                                            //move_object,
        rotate_object,                                          //rotate_object,
        display_in_circle_object,                               //display_in_circle_object,      
        display_symmetyrically_object,                          //display_symmetyrically_object,
        scale_object,                                           //scale_object,
                                                                ////операции с видом
        MoveView,                                               //move_camera_position,
        EnlargeView,                                            //zoom_in_camera,
        DiminishView,                                           //zoom_out_camera,
        ShowAll,                                                //show_all_objects,
                                                                ////операции со слоями
        AddLayer,                                               //add_layer,
        rename_layer,                                            //rename_layer,
        DeleteLayer,                                            //remove_layer,
                                                                //LayersManager,
        set_active_layer,                                       //set_active_layer,
        set_visible_layers,                                     //set_visible_layers,
        set_invisible_layers,                                   //set_invisible_layers,

        Count,
    }

    public class Layer
    {
        private int _id;
        private bool _visible;

        public Layer(bool visible = false)
        {
            _id = -1;
            _visible = visible;
        }

        public Layer(Layer layer)
        {
            _id = layer.Id;
            _visible = layer.Visible;
        }

        public Layer(int id, bool visible = false)
        {
            _id = id;
            _visible = visible;
        }

        public virtual int Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        public virtual bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
    }

    public class Document : IDisposable
    {
        #region Public

        public Document(Layer firstLayer)
        {
            Layers.Add(firstLayer);
            if (firstLayer.Id == -1)
                firstLayer.Id = _layersCounter++;
            Layers.CollectionChanged+=AddLayer;
        }

        public Document(IEnumerable<Layer> layers)
        {
            foreach (var l in layers)
                Layers.Add(l);
            Layers.CollectionChanged += AddLayer;
        }

        public Document(Document document)
        {
            _title = document.Title;
            _documentId = document.DocumentID;
            _disposed = document._disposed;
            Layers.Clear();
            foreach (var l in document.Layers)
                Layers.Add(l);
            Layers.CollectionChanged += AddLayer;
        }

        public virtual string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public virtual uint DocumentID
        {
            get { return _documentId; }
            set { _documentId = value; }
        }

        public virtual void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }

        public ObservableCollection<Layer> Layers
        {
            get { return _layers; }
        }

        public virtual Layer ActiveLayer
        {
            get { return _activeLayer ?? Layers.FirstOrDefault(); }
            set { _activeLayer = value; }
        }

        #endregion

        #region Protected

        #endregion

        #region Private

        private string _title;
        private uint _documentId;
        private bool _disposed;
        private readonly ObservableCollection<Layer> _layers = new ObservableCollection<Layer>();
        private Layer _activeLayer;
        private int _layersCounter;

        ~Document()
        {
            Dispose();
        }

        private void AddLayer(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if(notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add || notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Replace || notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Reset)
                foreach (Layer item in notifyCollectionChangedEventArgs.NewItems)
                    if (item.Id == -1)
                        item.Id = _layersCounter++;
        }

        #endregion
    }
    
    /*public class ApplicationController
    {
        //
        // Сводка:
        //     Specifies constants that define which mouse button was pressed.
        public enum MouseButtons
        {
            //
            // Сводка:
            //     No mouse button was pressed.
            None = 0,
            //
            // Сводка:
            //     The left mouse button was pressed.
            Left = 1048576,
            //
            // Сводка:
            //     The right mouse button was pressed.
            Right = 2097152,
            //
            // Сводка:
            //     The middle mouse button was pressed.
            Middle = 4194304,
            //
            // Сводка:
            //     The first XButton was pressed.
            XButton1 = 8388608,
            //
            // Сводка:
            //     The second XButton was pressed.
            XButton2 = 16777216
        }

        public enum Operations
        {
            OpPointCreate = 0,
            OpLineCreate,
            OpCircleCreate,
            OpContourCreate,
            OpDestroyContour,
            OpDeleteObject,
            OpUndo,
            OpRedo,
            OpSetLayersToShow,
            OpSetBackgroundColor,
        }

        #region Private

        private bool _leftButton; //for active document
        private bool _rightButton; //for active document
        private double _currentMouseCoordX; //for active document
        private double _currentMouseCoordY; //for active document
        private IntPtr _curSession = IntPtr.Zero; //temporary mock for View

        #endregion

        public ApplicationController()
        {
        }

        #region Public properties

        public Dictionary<uint, Document> Documents { get; } = new Dictionary<uint, Document>();

        #endregion

        public SessionId initSession()   //used when application running
        {
            _curSession = CoreWrapper.sessionFactory();
            SessionId sessionID = 0;
            return sessionID;
        }

        public uint initDocument(uint sessionID, IntPtr hwnd, Document document)  //used when creating new document
        {
            IntPtr pDoc = CoreWrapper.documentFactory(hwnd);
            DocumentId docID = CoreWrapper.attachDocument(_curSession, pDoc);
            if (Documents.ContainsKey(docID))
            {
                throw new ApplicationException("Document with same id already exists.");
            }
            document.DocumentID = docID;
            Documents[docID] = document;
            procOperation(sessionID, docID, Operations.OpSetLayersToShow,
                document.Layers.Select(l => (object) l.Id).ToArray());
            return docID;
        }

        public void eventHendling(uint docId, int action, double coordX = 0, double coordY = 0, double delta = 0)
        {
            _currentMouseCoordX = coordX;
            _currentMouseCoordY = coordY;
            switch ((MouseButtons) action)
            {
                case MouseButtons.Left:
                    _leftButton = true;
                    _rightButton = false;
                    break;
                case MouseButtons.Right:
                    _rightButton = true;
                    _leftButton = false;
                    break;
                default:
                    //Debug.Assert(false , "unknown event");
                    break;
            }
            if (delta != 0)
            {
                CoreWrapper.wheel(_curSession, docId, (int)delta);
            }
        }

        private OperationController startOperation(Operations opID)
        {
            OperationController operation = null;
            switch (opID)
            {
                case Operations.OpPointCreate:
                    operation = new OpPointCreate();
                    break;
                case Operations.OpLineCreate:
                    operation = new OpLineCreate();
                    break;
                case Operations.OpCircleCreate:
                    operation = new OpCircleCreate();
                    break;
                case Operations.OpContourCreate:
                    operation = new OpContourCreate();
                    break;
                case Operations.OpDestroyContour:
                    operation = new OpDestroyContour();
                    break;
                case Operations.OpDeleteObject:
                    operation = new OpDeleteObject();
                    break;
                case Operations.OpUndo:
                    operation = new OpUndo();
                    break;
                case Operations.OpRedo:
                    operation = new OpRedo();
                    break;
                case Operations.OpSetLayersToShow:
                    operation = new OpSetLayersToShow();
                    break;
                case Operations.OpSetBackgroundColor:
                    operation = new OpSetBackgroundColor();
                    break;
                default:
                    Debug.Assert(false, "unknown operation");
                    operation = new OperationController();
                    break;
            }
            return operation;
        }

        public void procOperation(SessionId sessionID, DocumentId docID, Operations opID, Object[] data)
        {
            OperationController curOperation = startOperation(opID);
            curOperation.Layer = Documents[docID].ActiveLayer.Id;
            curOperation.operationProcess(_curSession, docID, data);
        }

        public void finalDocument(SessionId sessionID, DocumentId docID) //closing the document
        {
            IntPtr document = CoreWrapper.detachDocument(_curSession, docID);
            CoreWrapper.destroyDocument(document);
            Documents.Remove(docID);
        }

        public void finalSession(SessionId sessionID)   //closing the application
        {
            //nothing yet
        }

        #region Core OpenGL draw functions

        public void draw(SessionId sessionID, DocumentId docID)
        {
            CoreWrapper.draw(_curSession, docID);
        }

        public void activateDocement(SessionId sessionID, DocumentId docID, int w, int h)
        {
            CoreWrapper.activateDocument(_curSession, docID, w, h);
        }

        public void resizeDocument(SessionId sessionID, DocumentId docID, int w, int h)
        {
            CoreWrapper.resizeDocument(_curSession, docID, w, h);
        }

        #endregion

    }

    class OpPointCreate : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            double X = (double)data[0];
            double Y = (double)data[1];
            
            IntPtr newNode = CoreWrapper.nodeFactory(X, Y);
            IntPtr newPoint = CoreWrapper.pointFactory(newNode);
            IntPtr newPointGen = CoreWrapper.genericFactory(newPoint, (uint)Layer);

            ObjectId newPointID = CoreWrapper.attachToBase(curSes, docID, newPointGen);
            CoreWrapper.commit(curSes, docID);

            //return newPointID;
        }
    }

    class OpCircleCreate : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            double X1 = (double)data[0];
            double Y1 = (double)data[1];
            double X2 = (double)data[2];
            double Y2 = (double)data[3];
            
            IntPtr center = CoreWrapper.nodeFactory(X1, Y1);
            IntPtr side = CoreWrapper.nodeFactory(X2, Y2);

            IntPtr newCircle = CoreWrapper.circleFactory(center, side);
            IntPtr newCircleGen = CoreWrapper.genericFactory(newCircle, (uint)Layer);

            ObjectId newCircleID = CoreWrapper.attachToBase(curSes, docID, newCircleGen);
            CoreWrapper.commit(curSes, docID);

            //return newCircleID;
        }
    }

    class OpContourCreate : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            IntPtr[] edges = new IntPtr[data.Length];
            uint currentLayer = CoreWrapper.getGenLayer(curSes, docID, (ObjectId)data[0]);

            for (uint i = 0; i < data.Length; ++i)
            {
                IntPtr curEdge = CoreWrapper.getGenTopology(curSes, docID, (ObjectId)data[i]);
                edges[i] = curEdge;
                CoreWrapper.detachFromBase(curSes, docID, (ObjectId)data[i]);
            }

            UnmanagedArray<IntPtr> edgesArray = new UnmanagedArray<IntPtr>(edges);
            IntPtr newContour = CoreWrapper.contourFactory(edgesArray, (uint)edgesArray.Size);
            IntPtr newContourGen = CoreWrapper.genericFactory(newContour, currentLayer);

            ObjectId newContourID = CoreWrapper.attachToBase(curSes, docID, newContourGen);
            CoreWrapper.commit(curSes, docID);

            //return newContourID;
        }
    }

    class OpDestroyContour : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            IntPtr temp = CoreWrapper.detachFromBase(curSes, docID, (ObjectId)data[0]);
            uint currentLayer = CoreWrapper.getGenericLayer(temp);
            IntPtr tempCon = CoreWrapper.getGenericTopology(temp);

            int size = 0;
            IntPtr pointer = IntPtr.Zero;
            pointer = CoreWrapper.getContourEdges(tempCon, ref size);

            IntPtr[] edges = new IntPtr[size];
            Marshal.Copy(pointer, edges, 0, size);

            for (uint i = 0; i < edges.Length; ++i)
            {
                IntPtr newEdgeGen = CoreWrapper.genericFactory(edges[i], currentLayer);
                CoreWrapper.attachToBase(curSes, docID, newEdgeGen);
            }

            CoreWrapper.commit(curSes, docID);
        }
    }

    class OpDeleteObject : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            CoreWrapper.detachFromBase(curSes, docID, (ObjectId)data[0]);
            CoreWrapper.commit(curSes, docID);
        }
    }

    class OpUndo : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            CoreWrapper.undo(curSes, docID);
        }
    }

    class OpRedo : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            CoreWrapper.redo(curSes, docID);
        }
    }

    class OpSetLayersToShow : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            UnmanagedArray<int> udata = new UnmanagedArray<int>(data.Cast<int>().ToArray());
            CoreWrapper.setLayers(curSes, docID, udata, udata.Size);
        }
    }

    class OpSetBackgroundColor : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            CoreWrapper.setBackgroundColor(curSes, docID, (Color)data[0]);
        }
    }*/
}
