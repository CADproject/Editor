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

Generic* Session::getGeneric(DOCID docID, OBJID objID)
{
	return getDocument(docID)->getGeneric(objID);
}

Topology* Session::getGenericTopology(DOCID docID, OBJID objID)
{
	return getDocument(docID)->getGeneric(objID)->getTopology();
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
	getDocument(docID)->toScreen();
}