#include <algorithm>
#include "controller.h"
#include "geometry.h"
#include "topology.h"

void createPoint(Session& curSes, DOCID docID, double X, double Y)
{
	Node node(X, Y);
	Point* newPoint = new Point(node);
	Generic newPointGen(newPoint);
	curSes.getDocument(docID)->attachToBase(&newPointGen);
}

void createLine(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node start(X1, Y1);
	Node end(X2, Y2);
	Line* newLine = new Line(start, end);
	Generic newLineGen(newLine);
	curSes.getDocument(docID)->attachToBase(&newLineGen);
}

void createCircle(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2)
{
	Node center(X1, Y1);
	Node side(X2, Y2);
	Circle* newCircle = new Circle(center, side);
	Generic newCircleGen(newCircle);
	curSes.getDocument(docID)->attachToBase(&newCircleGen);
}

void createContour(Session& curSes, DOCID docID, std::vector<OBJID> objects)
{
	std::vector<Edge*> edges;

	std::for_each(objects.begin(), objects.end(),
		[=, &curSes, &edges](OBJID objID)
	{
		Generic* temp = curSes.getDocument(docID)->getGeneric(objID);
		edges.push_back(dynamic_cast<Edge*>(temp->getTopology()));
	});

	Contour* newContour = new Contour(edges);
	Generic newContourGen(newContour);
	curSes.getDocument(docID)->attachToBase(&newContourGen);
}

void deletePoint(Session& curSes, DOCID docID, OBJID objID)
{
}

void deleteLine(Session& curSes, DOCID docID, OBJID objID)
{
}

void deleteCircle(Session& curSes, DOCID docID, OBJID objID)
{
}

void deleteContour(Session& curSes, DOCID docID, OBJID objID)
{
}

void destroyContour(Session& curSes, DOCID docID, OBJID objID)
{
}

void undo(Session& curSes, DOCID docID)
{
}

void redo(Session& curSes, DOCID docID)
{
}

void setLayers(Session& curSes, DOCID docID)
{
}

void setBackgroundColor(Session& curSes, DOCID docID)
{
}

void display(Session& curSes, DOCID docID)
{
}