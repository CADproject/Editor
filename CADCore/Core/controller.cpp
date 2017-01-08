#include <vector>
#include <algorithm>
#include "controller.h"
#include "geometry.h"
#include "topology.h"
#include "definitions.h"

OBJID createPoint(Session& curSes, DOCID docID, double X, double Y)
{
	Node node(X, Y);
	Point newPoint(node);

	Point* pNewPoint = &newPoint;
	Generic* newPointGen = new Generic(pNewPoint);
	
	OBJID newPointID = curSes.attachToBase(docID, newPointGen);
	curSes.commit(docID);

	return newPointID;
}

OBJID createLine(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node start(X1, Y1);
	Node end(X2, Y2);
	Line newLine(start, end);
	
	Line* pNewLine = &newLine;
	Generic* newLineGen = new Generic(pNewLine);

	OBJID newLineID = curSes.attachToBase(docID, newLineGen);
	curSes.commit(docID);

	return newLineID;
}

OBJID createCircle(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node center(X1, Y1);
	Node side(X2, Y2);
	Circle newCircle(center, side);
	
	Circle* pNewCircle = &newCircle;
	Generic* newCircleGen = new Generic(pNewCircle);
	
	OBJID newCircleID = curSes.attachToBase(docID, newCircleGen);
	curSes.commit(docID);

	return newCircleID;
}

OBJID createContour(Session& curSes, DOCID docID, std::vector<OBJID> objects)
{
	std::vector<Edge*> edges;

	std::for_each(objects.begin(), objects.end(),
		[=, &curSes, &edges](OBJID objID)
	{
		Edge* curEdge = dynamic_cast<Edge*>(curSes.getGenericTopology(docID, objID));
		edges.push_back(curEdge);
		Generic* temp = curSes.detachFromBase(docID, objID);
		delete temp;
	});

	Contour newContour(edges);
	
	Contour* pNewContour = &newContour;
	Generic* newContourGen = new Generic(pNewContour);

	OBJID newContourID = curSes.attachToBase(docID, newContourGen);
	curSes.commit(docID);

	return newContourID;
}

void deleteObject(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp = curSes.detachFromBase(docID, objID);
	delete temp;

	curSes.commit(docID);
}

void destroyContour(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp =curSes.detachFromBase(docID, objID);
	Contour* tempCon = dynamic_cast<Contour*>(temp->getTopology());

	std::list<Edge*> edges = tempCon->getEdges();
	std::for_each(edges.begin(), edges.end(),
		[=, &curSes](Edge* curEdge)
	{
		Generic* newEdgeGen = new Generic(curEdge);
		curSes.attachToBase(docID, newEdgeGen);
	});
		
	delete temp;

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

void setLayers(Session& curSes, DOCID docID, std::vector<unsigned>& layersToShow)
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