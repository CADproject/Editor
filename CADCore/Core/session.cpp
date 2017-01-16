#include "session.h"

unsigned Session::_counter;

DocumentId Session::attachDocument(Document* doc)
{
	++_counter;
	_session[_counter] = doc;
	return _counter;
}

void Session::detachDocument(DocumentId docID)
{
	auto iter = _session.find(docID);

	if(iter != _session.end())
		_session.erase(iter);
	else
		return;
}

Document* Session::getDocument(DocumentId docID)
{
	auto iter = _session.find(docID);
	if(iter != _session.end())
		return iter->second;
	else
		return nullptr;
}

ObjectId Session::attachToBase(DocumentId docID, Generic* object)
{
	return getDocument(docID)->attachToBase(object);
}

Generic* Session::detachFromBase(DocumentId docID, ObjectId objID)
{
	return getDocument(docID)->detachFromBase(objID);
}

void Session::attachToBuffer(DocumentId docID, Generic* object)
{
	getDocument(docID)->attachToBuffer(object);
}

void Session::detachFrombuffer(DocumentId docID, Generic* object)
{
	getDocument(docID)->detachFrombuffer(object);
}

Generic* Session::getGeneric(DocumentId docID, ObjectId objID)
{
	return getDocument(docID)->getGeneric(objID);
}

Topology* Session::getGenericTopology(DocumentId docID, ObjectId objID)
{
	return getDocument(docID)->getGeneric(objID)->getTopology();
}

unsigned Session::getGenericLayer(DocumentId docID, ObjectId objID)
{
	return getDocument(docID)->getGeneric(objID)->getLayer();
}

void Session::commit(DocumentId docID)
{
	getDocument(docID)->commit();
}

void Session::undo(DocumentId docID)
{
	getDocument(docID)->undo();
}

void Session::redo(DocumentId docID)
{
	getDocument(docID)->redo();
}

void Session::setLayers(DocumentId docID, std::vector<unsigned>& newLayers)
{
	getDocument(docID)->setLayers(newLayers);
}

void Session::setBackgroundColor(DocumentId docID, COLOR color)
{
	getDocument(docID)->setBackgroundColor(color);
}

void Session::toScreen(DocumentId docID)
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