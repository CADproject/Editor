/*This file contains controller sketch.*/
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CADController
{
    using SessionId = System.UInt32;
    using DocumentId = System.UInt32;
    //using OperationId = System.UInt32;
    using ObjectId = System.UInt32;

    public class ApplicationController
    {
        public enum mouseEvents : byte { leftButton, rightButton, move };

        public enum operations : byte { OpPointCreate,
                                        OpLineCreate,
                                        OpCircleCreate,
                                        OpContourCreate,
                                        OpDestroyContour,
                                        OpDeleteObject,
                                        OpUndo,
                                        OpRedo,
                                        OpSetLayersToShow,
                                        OpSetBackgroundColor };

        private bool _leftButton;        //for active document
        private bool _rightButton;       //for active document

        private double _currentMouseCoordX;  //for active document
        private double _currentMouseCoordY;  //for active document

        private IntPtr _curSession;  //temporary mock for View

        public ApplicationController()
        {
            _leftButton = false;
            _rightButton = false;

            _currentMouseCoordX = 0.0;
            _currentMouseCoordY = 0.0;

            _curSession = IntPtr.Zero;
        }
        
        public SessionId initSession()   //used when application running
        {
            _curSession = CoreWrapper.sessionFactory();
            SessionId sessionID = 0;
            return sessionID;
        }

        public uint initDocument(uint sessionID, IntPtr hwnd)  //used when creating new document
        {
            IntPtr pDoc = CoreWrapper.documentFactory(hwnd);
            DocumentId docID = CoreWrapper.attachDocument(_curSession, pDoc);
            return docID;
        }

        public void eventHendling(mouseEvents action, double coordX = 0, double coordY = 0)
        {
            switch (action)
            {
                case mouseEvents.leftButton:
                    _leftButton = true;
                    _rightButton = false;
                    break;
                case mouseEvents.rightButton:
                    _rightButton = true;
                    _leftButton = false;
                    break;
                case mouseEvents.move:
                    _currentMouseCoordX = coordX;
                    _currentMouseCoordY = coordY;
                    break;
                default:
                    Debug.Assert(false , "unknown event");
                    break;
            }
        }

        private OperationController startOperation(operations opID)
        {
            switch (opID)
            {
                case operations.OpPointCreate:
                    return new OpPointCreate();
                case operations.OpLineCreate:
                    return new OpLineCreate();
                case operations.OpCircleCreate:
                    return new OpCircleCreate();
                case operations.OpContourCreate:
                    return new OpContourCreate();
                case operations.OpDestroyContour:
                    return new OpDestroyContour();
                case operations.OpDeleteObject:
                    return new OpDeleteObject();
                case operations.OpUndo:
                    return new OpUndo();
                case operations.OpRedo:
                    return new OpRedo();
                case operations.OpSetLayersToShow:
                    return new OpSetLayersToShow();
                case operations.OpSetBackgroundColor:
                    return new OpSetBackgroundColor();
                default:
                    Debug.Assert(false, "unknown operation");
                    return new OperationController();
            }
        }

        public void procOperation(SessionId sessionID, DocumentId docID, operations opID, Object[] data)
        {
            OperationController curOperation = startOperation(opID);
            curOperation.operationProcess(_curSession, docID, data);
        }

        public void finalDocument(SessionId sessionID, DocumentId docID) //closing the document
        {
            IntPtr document = CoreWrapper.detachDocument(_curSession, docID);
            CoreWrapper.destroyDocument(document);
        }

        public void finalSession(SessionId sessionID)   //closing the application
        {
            //nothing yet
        }

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
    }

    class OperationController
    {
        public virtual void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            Debug.Assert(false, "This method without body");
        }
    }

    class OpPointCreate : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            double X = (double)data[0];
            double Y = (double)data[1];
            
            IntPtr newNode = CoreWrapper.nodeFactory(X, Y);
            IntPtr newPoint = CoreWrapper.pointFactory(newNode);
            IntPtr newPointGen = CoreWrapper.genericFactory(newPoint);

            ObjectId newPointID = CoreWrapper.attachToBase(curSes, docID, newPointGen);
            CoreWrapper.commit(curSes, docID);

            //return newPointID;
        }
    }

    class OpLineCreate : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            double X1 = (double)data[0];
            double Y1 = (double)data[1];
            double X2 = (double)data[2];
            double Y2 = (double)data[3];
            
            IntPtr start = CoreWrapper.nodeFactory(X1, Y1);
            IntPtr end = CoreWrapper.nodeFactory(X2, Y2);

            IntPtr newLine = CoreWrapper.lineFactory(start, end);
            IntPtr newLineGen = CoreWrapper.genericFactory(newLine);

            ObjectId newLineID = CoreWrapper.attachToBase(curSes, docID, newLineGen);
            CoreWrapper.commit(curSes, docID);

            //return newLineID;
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
            IntPtr newCircleGen = CoreWrapper.genericFactory(newCircle);

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

            UnmanagedArray edgesArray = new UnmanagedArray(edges);
            IntPtr newContour = CoreWrapper.contourFactory(edgesArray.getPointer(), (uint)edgesArray.getSize());
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
            CoreWrapper.setLayers(curSes, docID, (IntPtr)data[0]);
        }
    }

    class OpSetBackgroundColor : OperationController
    {
        public override void operationProcess(IntPtr curSes, DocumentId docID, Object[] data)
        {
            CoreWrapper.setBackgroundColor(curSes, docID, (Color)data[0]);
        }
    }

    class Operations    //temporary class (old code)
    {
        //create point
        public static ObjectId createPoint(IntPtr curSes, DocumentId docID, double X, double Y)
        {
        	IntPtr newNode = CoreWrapper.nodeFactory(X, Y);
            IntPtr newPoint = CoreWrapper.pointFactory(newNode);
        	IntPtr newPointGen = CoreWrapper.genericFactory(newPoint);
        	
        	ObjectId newPointID = CoreWrapper.attachToBase(curSes, docID, newPointGen);
        	CoreWrapper.commit(curSes, docID);
        
        	return newPointID;
        }

        //create line: start point, end point
        public static ObjectId createLine(IntPtr curSes, DocumentId docID, double X1, double Y1, double X2, double Y2)
        {
            IntPtr start = CoreWrapper.nodeFactory(X1, Y1);
            IntPtr end = CoreWrapper.nodeFactory(X2, Y2);

            IntPtr newLine = CoreWrapper.lineFactory(start, end);
            IntPtr newLineGen = CoreWrapper.genericFactory(newLine);

            ObjectId newLineID = CoreWrapper.attachToBase(curSes, docID, newLineGen);
            CoreWrapper.commit(curSes, docID);
        
        	return newLineID;
        }

        //create circle: center point, side point
        public static ObjectId createCircle(IntPtr curSes, DocumentId docID, double X1, double Y1, double X2, double Y2)
        {
        	IntPtr center = CoreWrapper.nodeFactory(X1, Y1);
            IntPtr side = CoreWrapper.nodeFactory(X2, Y2);

            IntPtr newCircle = CoreWrapper.circleFactory(center, side);
            IntPtr newCircleGen = CoreWrapper.genericFactory(newCircle);

            ObjectId newCircleID = CoreWrapper.attachToBase(curSes, docID, newCircleGen);
            CoreWrapper.commit(curSes, docID);
        
        	return newCircleID;
        }

        //create contour by existing edges
        public static unsafe ObjectId createContour(IntPtr curSes, DocumentId docID, ObjectId[] objectIDs)
        {
            IntPtr[] edges = new IntPtr[objectIDs.Length];
            uint currentLayer = CoreWrapper.getGenLayer(curSes, docID, objectIDs[0]);

            for (uint i = 0; i < objectIDs.Length; ++i)
            {
                IntPtr curEdge = CoreWrapper.getGenTopology(curSes, docID, objectIDs[i]);
        		edges[i] = curEdge;
                CoreWrapper.detachFromBase(curSes, docID, objectIDs[i]);
            }

            UnmanagedArray edgesArray = new UnmanagedArray(edges);
            IntPtr newContour = CoreWrapper.contourFactory(edgesArray.getPointer(), (uint)edgesArray.getSize());         
            IntPtr newContourGen = CoreWrapper.genericFactory(newContour, currentLayer);
        
        	ObjectId newContourID = CoreWrapper.attachToBase(curSes, docID, newContourGen);
        	CoreWrapper.commit(curSes, docID);

        	return newContourID;
        }
        
        //delete object by id
        public static void deleteObject(IntPtr curSes, DocumentId docID, ObjectId objID)
        {
            CoreWrapper.detachFromBase(curSes, docID, objID);
            CoreWrapper.commit(curSes, docID);
        }
        
        //destroy contour, all edges will be free
        public static void destroyContour(IntPtr curSes, DocumentId docID, ObjectId objID)
        {
        	IntPtr temp = CoreWrapper.detachFromBase(curSes, docID, objID);
            uint currentLayer = CoreWrapper.getGenericLayer(temp);
            IntPtr tempCon = CoreWrapper.getGenericTopology(temp);
                    
            int size = 0;
            IntPtr pointer = IntPtr.Zero;
            pointer = CoreWrapper.getContourEdges(tempCon, ref size);

            IntPtr[] edges = new IntPtr[size];
            Marshal.Copy(pointer, edges, 0, size);
                        
            for(uint i = 0; i < edges.Length; ++i)
            {
                IntPtr newEdgeGen = CoreWrapper.genericFactory(edges[i], currentLayer);
        		CoreWrapper.attachToBase(curSes, docID, newEdgeGen);
            }
        	
        	CoreWrapper.commit(curSes, docID);
        }
        
        //undo command
        public static void undo(IntPtr curSes, DocumentId docID)
        {
            CoreWrapper.undo(curSes, docID);
        }
        
        //redo command
        public static void redo(IntPtr curSes, DocumentId docID)
        {
            CoreWrapper.redo(curSes, docID);
        }
        
        //set layers to show
        public static void setLayersToShow(IntPtr curSes, DocumentId docID, IntPtr layersToShow)
        {
            CoreWrapper.setLayers(curSes, docID, layersToShow);
        }

        //set background color
        public static void setBackgroundColor(IntPtr curSes, DocumentId docID, Color newColor)
        {
            CoreWrapper.setBackgroundColor(curSes, docID, newColor);
        }

        //test operation - show and/or remove objects from controller
        public static void showAndRemoveFreeGeneric(IntPtr curSes, DocumentId docID)
        {
            IntPtr node = CoreWrapper.nodeFactory(33, 33);
            IntPtr newPoint = CoreWrapper.pointFactory(node);
            IntPtr newPointGen = CoreWrapper.genericFactory(newPoint);
            CoreWrapper.attachToBuffer(curSes, docID, newPointGen);
        }
    }
}