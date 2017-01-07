#include <vector>
#include <algorithm>
#include "controller.h"
#include "geometry.h"
#include "topology.h"
#include "definitions.h"

void createPoint(Session& curSes, DOCID docID, double X, double Y)
{
	Node node(X, Y);
	Point* newPoint = new Point(node);
	Generic newPointGen(newPoint);
	curSes.getDocument(docID)->attachToBase(&newPointGen);
	
	curSes.getDocument(docID)->commit();
}

void createLine(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node start(X1, Y1);
	Node end(X2, Y2);
	Line* newLine = new Line(start, end);
	Generic newLineGen(newLine);
	curSes.getDocument(docID)->attachToBase(&newLineGen);
	
	curSes.getDocument(docID)->commit();
}

void createCircle(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node center(X1, Y1);
	Node side(X2, Y2);
	Circle* newCircle = new Circle(center, side);
	Generic newCircleGen(newCircle);
	curSes.getDocument(docID)->attachToBase(&newCircleGen);
	
	curSes.getDocument(docID)->commit();
}

void createContour(Session& curSes, DOCID docID, std::vector<OBJID> objects)
{
	std::vector<Edge*> edges;

	std::for_each(objects.begin(), objects.end(),
		[=, &curSes, &edges](OBJID objID)
	{
		Generic* temp = curSes.getDocument(docID)->getGeneric(objID);
		curSes.getDocument(docID)->detachFromBase(objID);
		edges.push_back(dynamic_cast<Edge*>(temp->getTopology()));
	});

	Contour* newContour = new Contour(edges);
	Generic newContourGen(newContour);
	curSes.getDocument(docID)->attachToBase(&newContourGen);
	
	curSes.getDocument(docID)->commit();
}

void deletePoint(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp = curSes.getDocument(docID)->detachFromBase(objID);
	delete temp->getTopology();

	curSes.getDocument(docID)->commit();
}

void deleteLine(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp = curSes.getDocument(docID)->detachFromBase(objID);
	delete temp->getTopology();

	curSes.getDocument(docID)->commit();
}

void deleteCircle(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp = curSes.getDocument(docID)->detachFromBase(objID);
	delete temp->getTopology();

	curSes.getDocument(docID)->commit();
}

void deleteContour(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp = curSes.getDocument(docID)->detachFromBase(objID);
	Contour* tempCon = dynamic_cast<Contour*>(temp->getTopology());
	
	std::list<Edge*> edges = tempCon->getEdges();
	std::for_each(edges.begin(), edges.end(),
		[](Edge* curEdge) { delete curEdge; });

	delete tempCon;

	curSes.getDocument(docID)->commit();
}

void destroyContour(Session& curSes, DOCID docID, OBJID objID)
{
	Generic* temp = curSes.getDocument(docID)->detachFromBase(objID);
	Contour* tempCon = dynamic_cast<Contour*>(temp->getTopology());

	std::list<Edge*> edges = tempCon->getEdges();
	std::for_each(edges.begin(), edges.end(),
		[=, &curSes](Edge* curEdge)
	{
		Generic newEdgeGen(curEdge);
		curSes.getDocument(docID)->attachToBase(&newEdgeGen);
	});
		
	delete tempCon;

	curSes.getDocument(docID)->commit();
}

void undo(Session& curSes, DOCID docID)
{
	curSes.getDocument(docID)->undo();
}

void redo(Session& curSes, DOCID docID)
{
	curSes.getDocument(docID)->redo();
}

void setLayers(Session& curSes, DOCID docID, std::vector<unsigned>& layersToShow)
{
	curSes.getDocument(docID)->setLayers(layersToShow);
}

void setBackgroundColor(Session& curSes, DOCID docID, COLOR newColor)
{
	curSes.getDocument(docID)->setBackgroundColor(newColor);
}

void display(Session& curSes, DOCID docID)
{
	curSes.getDocument(docID)->toScreen();
}