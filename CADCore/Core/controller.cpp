#include <vector>
#include <algorithm>
#include "controller.h"
#include "geometry.h"
#include "topology.h"
#include "definitions.h"

ObjectId createPoint(Session& curSes, DocumentId docID, double X, double Y)
{
	Node node(X, Y);
	Point* newPoint = new Point(node);
	Generic* newPointGen = new Generic(newPoint);

	ObjectId newPointID = curSes.attachToBase(docID, newPointGen);
	curSes.commit(docID);

	return newPointID;
}

ObjectId createLine(Session& curSes, DocumentId docID, double X1, double Y1, double X2, double Y2)
{
	Node start(X1, Y1);
	Node end(X2, Y2);
	
	Line* newLine = new Line(start, end);
	Generic* newLineGen = new Generic(newLine);

	ObjectId newLineID = curSes.attachToBase(docID, newLineGen);
	curSes.commit(docID);

	return newLineID;
}

ObjectId createCircle(Session& curSes, DocumentId docID, double X1, double Y1, double X2, double Y2)
{
	Node center(X1, Y1);
	Node side(X2, Y2);
	
	Circle* newCircle = new Circle(center, side);
	Generic* newCircleGen = new Generic(newCircle);
	
	ObjectId newCircleID = curSes.attachToBase(docID, newCircleGen);
	curSes.commit(docID);

	return newCircleID;
}

ObjectId createContour(Session& curSes, DocumentId docID, std::vector<ObjectId> objects)
{
	std::vector<Edge*> edges;
	unsigned currentLayer = curSes.getGenericLayer(docID, objects.at(0));

	std::for_each(objects.begin(), objects.end(),
		[=, &curSes, &edges](ObjectId objID)
	{
		Edge* curEdge = dynamic_cast<Edge*>(curSes.getGenericTopology(docID, objID));
		edges.push_back(curEdge);
		curSes.detachFromBase(docID, objID);
	});
	
	Contour* newContour = new Contour(edges);
	Generic* newContourGen = new Generic(currentLayer, newContour);

	ObjectId newContourID = curSes.attachToBase(docID, newContourGen);
	curSes.commit(docID);

	return newContourID;
}

void deleteObject(Session& curSes, DocumentId docID, ObjectId objID)
{
	curSes.detachFromBase(docID, objID);
	curSes.commit(docID);
}

void destroyContour(Session& curSes, DocumentId docID, ObjectId objID)
{
	Generic* temp = curSes.detachFromBase(docID, objID);
	unsigned currentLayer = temp->getLayer();
	Contour* tempCon = dynamic_cast<Contour*>(temp->getTopology());

	std::list<Edge*> edges = tempCon->getEdges();
	std::for_each(edges.begin(), edges.end(),
		[=, &curSes](Edge* curEdge)
	{
		Generic* newEdgeGen = new Generic(currentLayer, curEdge);
		curSes.attachToBase(docID, newEdgeGen);
	});
	
	curSes.commit(docID);
}

void undo(Session& curSes, DocumentId docID)
{
	curSes.undo(docID);
}

void redo(Session& curSes, DocumentId docID)
{
	curSes.redo(docID);
}

void setLayersToShow(Session& curSes, DocumentId docID, std::vector<unsigned>& layersToShow)
{
	curSes.setLayers(docID, layersToShow);
}

void setBackgroundColor(Session& curSes, DocumentId docID, COLOR newColor)
{
	curSes.setBackgroundColor(docID, newColor);
}

void display(Session& curSes, DocumentId docID)
{
	curSes.toScreen(docID);
}

void showAndRemoveFreeGeneric(Session& curSes, DocumentId docID)
{
	Node node(33, 33);
	Point* newPoint = new Point(node);
	Generic* newPointGen = new Generic(newPoint);
	curSes.attachToBuffer(docID, newPointGen);
}