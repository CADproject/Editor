#include <vector>
#include <algorithm>
#include "controller.h"
#include "geometry.h"
#include "topology.h"
#include "definitions.h"

OBJID createPoint(Session& curSes, DOCID docID, double X, double Y)
{
	Node node(X, Y);
	Point* newPoint = new Point(node);
	Generic* newPointGen = new Generic(newPoint);

	OBJID newPointID = curSes.attachToBase(docID, newPointGen);
	curSes.commit(docID);

	return newPointID;
}

OBJID createLine(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node start(X1, Y1);
	Node end(X2, Y2);
	
	Line* newLine = new Line(start, end);
	Generic* newLineGen = new Generic(newLine);

	OBJID newLineID = curSes.attachToBase(docID, newLineGen);
	curSes.commit(docID);

	return newLineID;
}

OBJID createCircle(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node center(X1, Y1);
	Node side(X2, Y2);
	
	Circle* newCircle = new Circle(center, side);
	Generic* newCircleGen = new Generic(newCircle);
	
	OBJID newCircleID = curSes.attachToBase(docID, newCircleGen);
	curSes.commit(docID);

	return newCircleID;
}

OBJID createContour(Session& curSes, DOCID docID, std::vector<OBJID> objects)
{
	std::vector<Edge*> edges;
	unsigned currentLayer = curSes.getGenericLayer(docID, objects.at(0));

	std::for_each(objects.begin(), objects.end(),
		[=, &curSes, &edges](OBJID objID)
	{
		Edge* curEdge = dynamic_cast<Edge*>(curSes.getGenericTopology(docID, objID));
		edges.push_back(curEdge);
		curSes.detachFromBase(docID, objID);
	});
	
	Contour* newContour = new Contour(edges);
	Generic* newContourGen = new Generic(currentLayer, newContour);

	OBJID newContourID = curSes.attachToBase(docID, newContourGen);
	curSes.commit(docID);

	return newContourID;
}

void deleteObject(Session& curSes, DOCID docID, OBJID objID)
{
	curSes.detachFromBase(docID, objID);
	curSes.commit(docID);
}

void destroyContour(Session& curSes, DOCID docID, OBJID objID)
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

void undo(Session& curSes, DOCID docID)
{
	curSes.undo(docID);
}

void redo(Session& curSes, DOCID docID)
{
	curSes.redo(docID);
}

void setLayersToShow(Session& curSes, DOCID docID, std::vector<unsigned>& layersToShow)
{
	curSes.setLayers(docID, layersToShow);
}

void setBackgroundColor(Session& curSes, DOCID docID, COLOR newColor)
{
	curSes.setBackgroundColor(docID, newColor);
}

void display(Session& curSes, DOCID docID)
{
	curSes.toScreen(docID);
}

void showAndRemoveFreeGeneric(Session& curSes, DOCID docID)
{
	Node node(33, 33);
	Point* newPoint = new Point(node);
	Generic* newPointGen = new Generic(newPoint);
	curSes.attachToBuffer(docID, newPointGen);
}