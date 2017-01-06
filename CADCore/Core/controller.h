/*This temporary file contains controller sketch.*/

#include <vector>
#include "session.h"
#include "definitions.h"

//create point
void createPoint(Session& curSes, DOCID docID, double X, double Y);

//create line: start point, end point
void createLine(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2);

//create circle: center point, side point
void createCircle(Session& curSes, DOCID docID, double X1, double Y1, double X2, double Y2);

//create contour by existing edges
void createContour(Session& curSes, DOCID docID, std::vector<OBJID> objects);

//delete point by id
void deletePoint(Session& curSes, DOCID docID, OBJID objID);

//delete line by id
void deleteLine(Session& curSes, DOCID docID, OBJID objID);

//delete circle by id
void deleteCircle(Session& curSes, DOCID docID, OBJID objID);

//delete contour with all edges
void deleteContour(Session& curSes, DOCID docID, OBJID objID);

//destroy contour, all edges will be free
void destroyContour(Session& curSes, DOCID docID, OBJID objID);

//undo command
void undo(Session& curSes, DOCID docID);

//redo command
void redo(Session& curSes, DOCID docID);

//set layers to show
void setLayers(Session& curSes, DOCID docID);

//set background color
void setBackgroundColor(Session& curSes, DOCID docID);

//show the 2d editior field
void display(Session& curSes, DOCID docID);