/*This file contains the session class*/

#pragma once
#include <map>
#include "definitions.h"
#include "document.h"

//use Singleton from GOF catalogue (?)

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
	
	OBJID attachToBase(DOCID docID, Generic* object);
	Generic* detachFromBase(DOCID docID, OBJID objID);

	void attachToBuffer(DOCID docID, Generic* object);
	void detachFrombuffer(DOCID docID, Generic* object);

	Topology* getGenericTopology(DOCID docID, OBJID objID);
	unsigned getGenericLayer(DOCID docID, OBJID objID);

	void commit(DOCID docID);
	void undo(DOCID docID);
	void redo(DOCID docID);

	void setLayers(DOCID docID, std::vector<unsigned>& newLayers);
	void setBackgroundColor(DOCID docID, COLOR color);
	void toScreen(DOCID docID);

private:
	Document* getDocument(DOCID docID);
	Generic* getGeneric(DOCID docID, OBJID objID);
};