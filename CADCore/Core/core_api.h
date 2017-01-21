#pragma once
#include "headers.h"
#include "session.h"

#define COREDLL_API __declspec(dllexport)

//session factory and all methods
extern "C" COREDLL_API void* sessionFactory(void);
extern "C" COREDLL_API DocumentId attachDocument(void* pObject, Document* doc);
extern "C" COREDLL_API ObjectId attachToBase(void* pObject, DocumentId docID, Generic* object);
extern "C" COREDLL_API void* detachFromBase(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API void attachToBuffer(void* pObject, DocumentId docID, Generic* object);
extern "C" COREDLL_API void* getGenericTopology(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API unsigned getGenericLayer(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API void setLayers(void* pObject, DocumentId docID, std::vector<unsigned>& newLayers);
extern "C" COREDLL_API void setBackgroundColor(void* pObject, DocumentId docID, COLOR color);
extern "C" COREDLL_API void toScreen(void* pObject, DocumentId docID);
extern "C" COREDLL_API void commit(void* pObject, DocumentId docID);
extern "C" COREDLL_API void undo(void* pObject, DocumentId docID);
extern "C" COREDLL_API void redo(void* pObject, DocumentId docID);

//document factory method
extern "C" COREDLL_API void* documentFactory(void);

//node factory method
extern "C" COREDLL_API void* nodeFactory(double x, double y);

//point factory method
extern "C" COREDLL_API void* pointFactory(const Node& node);

//line factory method
extern "C" COREDLL_API void* lineFactory(const Node& start, const Node& end);

//circle factory method
extern "C" COREDLL_API void* circleFactory(const Node& center, const Node& side);

//contour factory method
extern "C" COREDLL_API void* contourFactory(const std::vector<Edge*>& edges);
extern "C" COREDLL_API void* getEdges(void* pObject);

//generic factory method
extern "C" COREDLL_API void* genericFactory(Topology* primitive, unsigned layer = 0, COLOR color = BLACK, THICKNESS thickness = THREE);
extern "C" COREDLL_API unsigned getLayer(void* pObject);
extern "C" COREDLL_API void* getTopology(void* pObject);