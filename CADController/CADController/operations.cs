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

//#include "controller.h"
//
//ObjectId createPoint(Session& curSes, DocumentId docID, double X, double Y)
//{
//	Node* newNode = nodeFactory(X, Y);
//	Point* newPoint = pointFactory(*newNode);
//	Generic* newPointGen = genericFactory(newPoint);
//	
//	ObjectId newPointID = curSes.attachToBase(docID, newPointGen);
//	curSes.commit(docID);
//
//	return newPointID;
//}
//
//ObjectId createLine(Session& curSes, DocumentId docID, double X1, double Y1, double X2, double Y2)
//{
//	Node* start = nodeFactory(X1, Y1);
//	Node* end = nodeFactory(X2, Y2);
//	
//	Line* newLine = lineFactory(*start, *end);
//	Generic* newLineGen = genericFactory(newLine);
//
//	ObjectId newLineID = curSes.attachToBase(docID, newLineGen);
//	curSes.commit(docID);
//
//	return newLineID;
//}
//
//ObjectId createCircle(Session& curSes, DocumentId docID, double X1, double Y1, double X2, double Y2)
//{
//	Node* center = nodeFactory(X1, Y1);
//	Node* side = nodeFactory(X2, Y2);
//	
//	Circle* newCircle = circleFactory(*center, *side);
//	Generic* newCircleGen = genericFactory(newCircle);
//	
//	ObjectId newCircleID = curSes.attachToBase(docID, newCircleGen);
//	curSes.commit(docID);
//
//	return newCircleID;
//}
//
//ObjectId createContour(Session& curSes, DocumentId docID, std::vector<ObjectId> objects)
//{	
//	std::vector<Edge*> edges;
//	unsigned currentLayer = curSes.getGenericLayer(docID, objects.at(0));
//
//	std::for_each(objects.begin(), objects.end(),
//		[=, &curSes, &edges](ObjectId objID)
//	{
//		Edge* curEdge = dynamic_cast<Edge*>(curSes.getGenericTopology(docID, objID));
//		edges.push_back(curEdge);
//		curSes.detachFromBase(docID, objID);
//	});
//	
//	Contour* newContour = contourFactory(edges);
//	Generic* newContourGen = genericFactory(currentLayer, newContour);
//	
//	ObjectId newContourID = curSes.attachToBase(docID, newContourGen);
//	curSes.commit(docID);
//
//	return newContourID;
//}
//
//void deleteObject(Session& curSes, DocumentId docID, ObjectId objID)
//{
//	curSes.detachFromBase(docID, objID);
//	curSes.commit(docID);
//}
//
//void destroyContour(Session& curSes, DocumentId docID, ObjectId objID)
//{
//	Generic* temp = curSes.detachFromBase(docID, objID);
//	unsigned currentLayer = temp->getLayer();
//	Contour* tempCon = dynamic_cast<Contour*>(temp->getTopology());
//
//	std::list<Edge*> edges = tempCon->getEdges();
//	std::for_each(edges.begin(), edges.end(),
//		[=, &curSes](Edge* curEdge)
//	{
//		Generic* newEdgeGen = genericFactory(currentLayer, curEdge);
//		curSes.attachToBase(docID, newEdgeGen);
//	});
//	
//	curSes.commit(docID);
//}
//
//void undo(Session& curSes, DocumentId docID)
//{
//	curSes.undo(docID);
//}
//
//void redo(Session& curSes, DocumentId docID)
//{
//	curSes.redo(docID);
//}
//
//void setLayersToShow(Session& curSes, DocumentId docID, std::vector<unsigned>& layersToShow)
//{
//	curSes.setLayers(docID, layersToShow);
//}
//
//void setBackgroundColor(Session& curSes, DocumentId docID, COLOR newColor)
//{
//	curSes.setBackgroundColor(docID, newColor);
//}
//
//void display(Session& curSes, DocumentId docID)
//{
//	curSes.toScreen(docID);
//}
//
//void showAndRemoveFreeGeneric(Session& curSes, DocumentId docID)
//{
//	Node* node = nodeFactory(33, 33);
//	Point* newPoint = pointFactory(*node);
//	Generic* newPointGen = genericFactory(newPoint);
//	curSes.attachToBuffer(docID, newPointGen);
//}