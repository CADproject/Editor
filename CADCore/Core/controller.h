/*This temporary file contains controller sketch.*/

#include <vector>
#include "session.h"
#include "definitions.h"

//create point
OBJID createPoint(Session& curSes, DOCID docID, double X, double Y);

//create line: start point, end point
OBJID createLine(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2);

//create circle: center point, side point
OBJID createCircle(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2);

//create contour by existing edges
OBJID createContour(Session& curSes, DOCID docID, std::vector<OBJID> objects);

//delete point by id
void deleteObject(Session& curSes, DOCID docID, OBJID objID);

//destroy contour, all edges will be free
void destroyContour(Session& curSes, DOCID docID, OBJID objID);

//undo command
void undo(Session& curSes, DOCID docID);

//redo command
void redo(Session& curSes, DOCID docID);

//set layers to show
void setLayersToShow(Session& curSes, DOCID docID, std::vector<unsigned>& layersToShow);

//set background color
void setBackgroundColor(Session& curSes, DOCID docID, COLOR newColor);

//show the 2d editior field
void display(Session& curSes, DOCID docID);

//test operation - show and/or remove objects not from base
void showAndRemoveFreeGeneric(Session& curSes, DOCID docID);