#include "document.h"

OBJID Document::attachToBase(Generic* object)
{
	return _base.attachToBase(object);
}

void Document::detachFromBase(OBJID objID)
{
	_base.detachFromBase(objID);
}

Generic* Document::getGeneric(OBJID objID)
{
	return _base.getGeneric(objID);
}

void Document::undo(void)
{
	_base.undo();
}

void Document::redo(void)
{
	_base.redo();
}