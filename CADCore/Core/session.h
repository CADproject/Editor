/*This file contains the session class*/

#pragma once
#include <map>
#include "definitions.h"
#include "document.h"

class Session
{
private:
	static unsigned _counter;				//documents counter
	std::map<DOCID, Document*> _session;	//all open documents

public:
	Session() {}
	~Session() {}

	DOCID attachDocument(Document* doc);
	void detachDocument(DOCID docID);
	Document* getDocument(DOCID docID);
};