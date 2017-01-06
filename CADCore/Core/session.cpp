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