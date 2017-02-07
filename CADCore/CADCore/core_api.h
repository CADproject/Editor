#pragma once
#include "headers.h"
#include "session.h"

#define COREDLL_API __declspec(dllexport)

//session factory and all methods
extern "C" COREDLL_API void* sessionFactory(void);
extern "C" COREDLL_API DocumentId attachDocument(void* pObject, void* doc);
extern "C" COREDLL_API void detachDocument(void* pObject, DocumentId docID);
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
extern "C" COREDLL_API void* contourFactory(void** pEdges, unsigned size);
extern "C" COREDLL_API void** getContourEdges(void* pObject, int& size);

//generic factory method
extern "C" COREDLL_API void* genericFactory(void* primitive, unsigned layer = 0, COLOR color = BLACK, THICKNESS thickness = THREE);
extern "C" COREDLL_API unsigned getGenericLayer(void* pObject);
extern "C" COREDLL_API void* getGenericTopology(void* pObject);