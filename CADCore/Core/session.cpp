#include <iostream>
#include <algorithm>
#include "session.h"

unsigned Session::_counter;

DOCID Session::attachDocument(Document* doc)
{
	++_counter;
	_session[_counter] = doc;
	return _counter;
}

void Session::detachDocument(DOCID docID)
{
	auto iter = _session.find(docID);

	if(iter != _session.end())
		_session.erase(iter);
	else
		return;
}

Document* Session::getDocument(DOCID docID)
{
	auto iter = _session.find(docID);
	if(iter != _session.end())
		return iter->second;
	else
		return nullptr;
}

OBJID Session::attachToBase(DOCID docID, Generic* object)
{
	return getDocument(docID)->attachToBase(object);
}

Generic* Session::detachFromBase(DOCID docID, OBJID objID)
{
	return getDocument(docID)->detachFromBase(objID);
}

void Session::attachToBuffer(DOCID docID, Generic* object)
{
	getDocument(docID)->attachToBuffer(object);
}

void Session::detachFrombuffer(DOCID docID, Generic* object)
{
	getDocument(docID)->detachFrombuffer(object);
}

Generic* Session::getGeneric(DOCID docID, OBJID objID)
{
	return getDocument(docID)->getGeneric(objID);
}

Topology* Session::getGenericTopology(DOCID docID, OBJID objID)
{
	return getDocument(docID)->getGeneric(objID)->getTopology();
}

unsigned Session::getGenericLayer(DOCID docID, OBJID objID)
{
	return getDocument(docID)->getGeneric(objID)->getLayer();
}

void Session::commit(DOCID docID)
{
	getDocument(docID)->commit();
}

void Session::undo(DOCID docID)
{
	getDocument(docID)->undo();
}

void Session::redo(DOCID docID)
{
	getDocument(docID)->redo();
}

void Session::setLayers(DOCID docID, std::vector<unsigned>& newLayers)
{
	getDocument(docID)->setLayers(newLayers);
}

void Session::setBackgroundColor(DOCID docID, COLOR color)
{
	getDocument(docID)->setBackgroundColor(color);
}

void Session::toScreen(DOCID docID)
{
	std::cout << "DOCUMENT: " << docID << ". ";
	std::cout << "Layers (on screen):";
	
	std::vector<unsigned> layers = getDocument(docID)->getLayers();
	std::for_each(layers.begin(), layers.end(),
		[](unsigned number)
	{
		std::cout << " " << number;
	});

	std::cout << ". Background color: ";
	std::cout << getDocument(docID)->getBackgroundColor() << "." << std::endl;
	
	getDocument(docID)->toScreen();
}