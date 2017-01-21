#include "core_api.h"

void* sessionFactory(void)
{
	return static_cast<void*>(new Session());
}

DocumentId attachDocument(void* pObject, Document* doc)
{
	Session* ses = static_cast<Session*>(pObject);
	return ses->attachDocument(doc);
}

ObjectId attachToBase(void* pObject, DocumentId docID, Generic* object)
{
	Session* ses = static_cast<Session*>(pObject);
	return ses->attachToBase(docID, object);
}

void* detachFromBase(void* pObject, DocumentId docID, ObjectId objID)
{
	Session* ses = static_cast<Session*>(pObject);
	return static_cast<void*>(ses->detachFromBase(docID, objID));
}

void attachToBuffer(void* pObject, DocumentId docID, Generic* object)
{
	Session* ses = static_cast<Session*>(pObject);
	ses->attachToBuffer(docID, object);
}

void* getGenericTopology(void* pObject, DocumentId docID, ObjectId objID)
{
	Session* ses = static_cast<Session*>(pObject);
	return static_cast<void*>(ses->getGenericTopology(docID, objID));
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
	return new Document();
}

void* nodeFactory(double x, double y)
{
	return static_cast<void*>(new Node(x, y));
}

void* pointFactory(const Node& node)
{
	return static_cast<void*>(new Point(node));
}

void* lineFactory(const Node& start, const Node& end)
{
	return static_cast<void*>(new Line(start, end));
}

void* circleFactory(const Node& center, const Node& side)
{
	return static_cast<void*>(new Circle(center, side));
}

void* contourFactory(const std::vector<Edge*>& edges)
{
	return static_cast<void*>(new Contour(edges));
}

void* getEdges(void* pObject)
{
	Contour* cont = static_cast<Contour*>(pObject);
	return static_cast<void*>(&(cont->getEdges()));
}

void* genericFactory(Topology* primitive, unsigned layer, COLOR color, THICKNESS thickness)
{
	if(layer == 0 && color == BLACK && thickness == THREE)
	{
		return static_cast<void*>(new Generic(primitive));
	}
	else if(color == BLACK && thickness == THREE)
	{
		return static_cast<void*>(new Generic(layer, primitive));
	}
	else
	{
		return static_cast<void*>(new Generic(layer, color, thickness, primitive));
	}
}

unsigned getLayer(void* pObject)
{
	Generic* gen = static_cast<Generic*>(pObject);
	return gen->getLayer();
}

void* getTopology(void* pObject)
{
	Generic* gen = static_cast<Generic*>(pObject);
	return static_cast<void*>(gen->getTopology());
}