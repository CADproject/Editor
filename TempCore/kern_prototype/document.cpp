#include "headers.h"
#include "document.h"

void Document::addNodeToModelTree(ConnectData& source, FEATID featureID)
{
	_tree.addNode(source, featureID);
}

void Document::selectLayers(vector<unsigned>& layersToShow)
{ 
	_buffer.setLayers(layersToShow); 
}

void Document::changeThickness(OBJID id, THICKNESS newThickness)
{
	_tree.getBase().getGenericObject(id)->setThickness(newThickness);
}

void Document::changeColor(OBJID id, COLOR newColor)
{
	_tree.getBase().getGenericObject(id)->setColor(newColor);
}

void Document::undo(void) { _undoredo.undo(); }

void Document::redo(void) { _undoredo.redo(); }

void Document::changeCacheUndoRedo(unsigned cache) { _undoredo.setSize(cache); }

PointStruct Document::getPointCoords(OBJID id)
{
	Point* pointObj = dynamic_cast<Point*>( _tree.getBase().getGenericObject(id)->getGeometry() );
	return pointObj->getPointCoords();
}