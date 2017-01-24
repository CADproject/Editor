#include "core_api.h"

void* sessionFactory(void)
{
	return static_cast<void*>(new Session());
}

DocumentId attachDocument(void* pObject, void* doc)
{
	Session* ses = static_cast<Session*>(pObject);
	Document* pDoc = static_cast<Document*>(doc);
	return ses->attachDocument(pDoc);
}

ObjectId attachToBase(void* pObject, DocumentId docID, void* gen)
{
	Session* ses = static_cast<Session*>(pObject);
	Generic* pGen = static_cast<Generic*>(gen);
	return ses->attachToBase(docID, pGen);
}

void* detachFromBase(void* pObject, DocumentId docID, ObjectId objID)
{
	Session* ses = static_cast<Session*>(pObject);
	return static_cast<void*>(ses->detachFromBase(docID, objID));
}

void attachToBuffer(void* pObject, DocumentId docID, void* gen)
{
	Session* ses = static_cast<Session*>(pObject);
	Generic* pGen = static_cast<Generic*>(gen);
	ses->attachToBuffer(docID, pGen);
}

void* getGenTopology(void* pObject, DocumentId docID, ObjectId objID)
{
	Session* ses = static_cast<Session*>(pObject);
	return static_cast<void*>(ses->getGenTopology(docID, objID));
}

unsigned getGenLayer(void* pObject, DocumentId docID, ObjectId objID)
{
	Session* ses = static_cast<Session*>(pObject);
	return ses->getGenLayer(docID, objID);
}

void setLayers(void* pObject, DocumentId docID, void* pNewLayers)
{
	Session* ses = static_cast<Session*>(pObject);
	auto pNL = static_cast<std::vector<unsigned>*>(pNewLayers); 
	ses->setLayers(docID, *pNL);
}

void setBackgroundColor(void* pObject, DocumentId docID, COLOR color)
{
	Session* ses = static_cast<Session*>(pObject);
	ses->setBackgroundColor(docID, color);
}

void toScreen(void* pObject, DocumentId docID)
{
	Session* ses = static_cast<Session*>(pObject);
	ses->toScreen(docID);
}

void commit(void* pObject, DocumentId docID)
{
	Session* ses = static_cast<Session*>(pObject);
	ses->commit(docID);
}

void undo(void* pObject, DocumentId docID)
{
	Session* ses = static_cast<Session*>(pObject);
	ses->undo(docID);
}

void redo(void* pObject, DocumentId docID)
{
	Session* ses = static_cast<Session*>(pObject);
	ses->redo(docID);
}

void* documentFactory(void)
{
	return static_cast<void*>(new Document());
}

void* nodeFactory(double x, double y)
{
	return static_cast<void*>(new Node(x, y));
}

void* pointFactory(void* node)
{
	Node* pNode = static_cast<Node*>(node);

	return static_cast<void*>(new Point(*pNode));
}

void* lineFactory(void* start, void* end)
{
	Node* pStart = static_cast<Node*>(start);
	Node* pEnd = static_cast<Node*>(end);

	return static_cast<void*>(new Line(*pStart, *pEnd));
}

void* circleFactory(void* center, void* side)
{
	Node* pCenter = static_cast<Node*>(center);
	Node* pSide = static_cast<Node*>(side);

	return static_cast<void*>(new Circle(*pCenter, *pSide));
}

void* contourFactory(void* pEdges)
{
	auto pEd = static_cast<std::vector<Edge*>*>(pEdges); 
	return static_cast<void*>(new Contour(*pEd));
}

void* getContourEdges(void* pObject)
{
	Contour* cont = static_cast<Contour*>(pObject);
	return static_cast<void*>(cont->getEdges());
}

void* genericFactory(void* primitive, unsigned layer, COLOR color, THICKNESS thickness)
{
	Topology* prim = static_cast<Topology*>(primitive);

	if(layer == 0 && color == BLACK && thickness == THREE)
	{
		return static_cast<void*>(new Generic(prim));
	}
	else if(color == BLACK && thickness == THREE)
	{
		return static_cast<void*>(new Generic(layer, prim));
	}
	else
	{
		return static_cast<void*>(new Generic(layer, color, thickness, prim));
	}
}

unsigned getGenericLayer(void* pObject)
{
	Generic* gen = static_cast<Generic*>(pObject);
	return gen->getLayer();
}

void* getGenericTopology(void* pObject)
{
	Generic* gen = static_cast<Generic*>(pObject);
	return static_cast<void*>(gen->getTopology());
}

void* createVector(void)
{
	return static_cast<void*>(new std::vector<unsigned>()); 
}

void deleteVector(void* pObject)
{
	auto temp = static_cast<std::vector<unsigned>*>(pObject);
	delete temp;
}

void push_back_unsigned(void* pObject, unsigned value)
{
	auto temp = static_cast<std::vector<unsigned>*>(pObject);
	temp->push_back(value);
}

void push_back_edge(void* pObject, void* value)
{
	auto temp = static_cast<std::vector<Edge*>*>(pObject);
	auto pEdge = static_cast<Edge*>(value);
	temp->push_back(pEdge);
}

void pop_back(void* pObject)
{
	auto temp = static_cast<std::vector<unsigned>*>(pObject);
	temp->pop_back();
}

void clear(void* pObject)
{
	auto temp = static_cast<std::vector<unsigned>*>(pObject);
	temp->clear();
}

unsigned at_unsigned(void* pObject, unsigned index)
{
	auto temp = static_cast<std::vector<unsigned>*>(pObject);
	return temp->at(index);
}

void* at_edge(void* pObject, unsigned index)
{
	auto temp = static_cast<std::vector<Edge*>*>(pObject);
	return static_cast<void*>(temp->at(index));
}

unsigned size(void* pObject)
{
	auto temp = static_cast<std::vector<unsigned>*>(pObject);
	return temp->size();
}