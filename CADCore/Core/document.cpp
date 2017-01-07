#include "document.h"

OBJID Document::attachToBase(Generic* object)
{
	return _base.attachToBase(object);
}

Generic* Document::detachFromBase(OBJID objID)
{
	return _base.detachFromBase(objID);
}

Generic* Document::getGeneric(OBJID objID)
{
	return _base.getGeneric(objID);
}

void Document::commit(void)
{
	_base.commit();
}

void Document::undo(void)
{
	_base.undo();
}

void Document::redo(void)
{
	_base.redo();
}

void Document::setLayers(std::vector<unsigned>& newLayers)
{
	_buffer.setLayers(newLayers);
}

void Document::setBackgroundColor(COLOR color)
{
	_settings.setBackgroundColor(color);
}

void Document::toScreen(void)
{
	_buffer.toScreen();
}