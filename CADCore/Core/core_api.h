#pragma once
#include "headers.h"
#include "session.h"

#define COREDLL_API __declspec(dllexport)

//session factory and all methods
extern "C" COREDLL_API void* sessionFactory(void);
extern "C" COREDLL_API DocumentId attachDocument(void* pObject, void* doc);
extern "C" COREDLL_API ObjectId attachToBase(void* pObject, DocumentId docID, void* gen);
extern "C" COREDLL_API void* detachFromBase(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API void attachToBuffer(void* pObject, DocumentId docID, void* gen);
extern "C" COREDLL_API void* getGenTopology(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API unsigned getGenLayer(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API void setLayers(void* pObject, DocumentId docID, void* newLayers);
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
extern "C" COREDLL_API void* pointFactory(void* node);

//line factory method
extern "C" COREDLL_API void* lineFactory(void* start, void* end);

//circle factory method
extern "C" COREDLL_API void* circleFactory(void* center, void* side);

//contour factory method
extern "C" COREDLL_API void* contourFactory(void* pEdges);
extern "C" COREDLL_API void* getEdges(void* pObject);

//generic factory method
extern "C" COREDLL_API void* genericFactory(void* primitive, unsigned layer = 0, COLOR color = BLACK, THICKNESS thickness = THREE);
extern "C" COREDLL_API unsigned getGenericLayer(void* pObject);
extern "C" COREDLL_API void* getGenericTopology(void* pObject);

//STL vector for C#
extern "C" COREDLL_API void* createVector(void);
extern "C" COREDLL_API void deleteVector(void* pObject);
extern "C" COREDLL_API void push_back_unsigned(void* pObject, unsigned value);
extern "C" COREDLL_API void push_back_edge(void* pObject, void* value);
extern "C" COREDLL_API void pop_back(void* pObject);
extern "C" COREDLL_API void clear(void* pObject);
extern "C" COREDLL_API unsigned at_unsigned(void* pObject, unsigned index);
extern "C" COREDLL_API void* at_edge(void* pObject, unsigned index);
extern "C" COREDLL_API unsigned size(void* pObject);