/*This file contains the session class*/

#pragma once
#include "headers.h"
#include "document.h"

//use Singleton from GOF catalogue (?)

class Session
{
private:
	static unsigned _counter;			//documents counter
	std::map<DocumentId, Document*> _session;	//all open documents

public:
	Session() {}
	~Session() {}

	DocumentId attachDocument(Document* doc);
	void detachDocument(DocumentId docID);
	
	ObjectId attachToBase(DocumentId docID, Generic* object);
	Generic* detachFromBase(DocumentId docID, ObjectId objID);

	void attachToBuffer(DocumentId docID, Generic* object);
	void detachFrombuffer(DocumentId docID, Generic* object);

	Topology* getGenTopology(DocumentId docID, ObjectId objID);
	unsigned getGenLayer(DocumentId docID, ObjectId objID);

	void commit(DocumentId docID);
	void undo(DocumentId docID);
	void redo(DocumentId docID);

	void setLayers(DocumentId docID, std::vector<unsigned>& newLayers);
	void setBackgroundColor(DocumentId docID, COLOR color);
	void toScreen(DocumentId docID);

private:
	Document* getDocument(DocumentId docID);
	Generic* getGeneric(DocumentId docID, ObjectId objID);
};