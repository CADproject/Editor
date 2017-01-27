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
extern "C" COREDLL_API void* getContourEdges(void* pObject);

//generic factory method
extern "C" COREDLL_API void* genericFactory(void* primitive, unsigned layer = 0, COLOR color = BLACK, THICKNESS thickness = THREE);
extern "C" COREDLL_API unsigned getGenericLayer(void* pObject);
extern "C" COREDLL_API void* getGenericTopology(void* pObject);

//STL vector unsigned for C#
extern "C" COREDLL_API void* createVectorU(void);
extern "C" COREDLL_API void deleteVectorU(void* pObject);
extern "C" COREDLL_API void push_backU(void* pObject, unsigned value);
extern "C" COREDLL_API void pop_backU(void* pObject);
extern "C" COREDLL_API void clearU(void* pObject);
extern "C" COREDLL_API unsigned atU(void* pObject, unsigned index);
extern "C" COREDLL_API unsigned sizeU(void* pObject);

//STL vector of edges for C#
extern "C" COREDLL_API void* createVectorE(void);
extern "C" COREDLL_API void deleteVectorE(void* pObject);
extern "C" COREDLL_API void push_backE(void* pObject, void* value);
extern "C" COREDLL_API void pop_backE(void* pObject);
extern "C" COREDLL_API void clearE(void* pObject);
extern "C" COREDLL_API void* atE(void* pObject, unsigned index);
extern "C" COREDLL_API unsigned sizeE(void* pObject);