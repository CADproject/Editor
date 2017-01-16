///*This temporary file contains controller sketch.*/
//
//#pragma once
//#include "headers.h"
//#include "core_api.h"
//
////create point
//ObjectId createPoint(Session& curSes, DocumentId docID, double X, double Y);
//
////create line: start point, end point
//ObjectId createLine(Session& curSes, DocumentId docID, double X1, double Y1, double X2, double Y2);
//
////create circle: center point, side point
//ObjectId createCircle(Session& curSes, DocumentId docID, double X1, double Y1, double X2, double Y2);
//
////create contour by existing edges
//ObjectId createContour(Session& curSes, DocumentId docID, std::vector<ObjectId> objects);
//
////delete point by id
//void deleteObject(Session& curSes, DocumentId docID, ObjectId objID);
//
////destroy contour, all edges will be free
//void destroyContour(Session& curSes, DocumentId docID, ObjectId objID);
//
////undo command
//void undo(Session& curSes, DocumentId docID);
//
////redo command
//void redo(Session& curSes, DocumentId docID);
//
////set layers to show
//void setLayersToShow(Session& curSes, DocumentId docID, std::vector<unsigned>& layersToShow);
//
////set background color
//void setBackgroundColor(Session& curSes, DocumentId docID, COLOR newColor);
//
////show the 2d editior field
//void display(Session& curSes, DocumentId docID);
//
////test operation - show and/or remove objects from controller
//void showAndRemoveFreeGeneric(Session& curSes, DocumentId docID);