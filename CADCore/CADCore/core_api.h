#pragma once
#include "headers.h"
#include "session.h"

#define COREDLL_CALL __stdcall
#define COREDLL_API  __declspec(dllexport)

//session factory and all methods
extern "C" COREDLL_API void* sessionFactory(void);
extern "C" COREDLL_API DocumentId attachDocument(void* pObject, void* doc);
extern "C" COREDLL_API void* detachDocument(void* pObject, DocumentId docID);
extern "C" COREDLL_API void destroyDocument(void* pObject);
extern "C" COREDLL_API ObjectId attachToBase(void* pObject, DocumentId docID, void* gen);
extern "C" COREDLL_API void* detachFromBase(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API void attachToBuffer(void* pObject, DocumentId docID, void* gen);
extern "C" COREDLL_API void* getGenTopology(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API unsigned getGenLayer(void* pObject, DocumentId docID, ObjectId objID);
extern "C" COREDLL_API void setLayers(void* pObject, DocumentId docID, void* pNewLayers, unsigned size);
extern "C" COREDLL_API void setBackgroundColor(void* pObject, DocumentId docID, COLOR color);
extern "C" COREDLL_API void toScreen(void* pObject, DocumentId docID);	//(?)
extern "C" COREDLL_API void commit(void* pObject, DocumentId docID);
extern "C" COREDLL_API void undo(void* pObject, DocumentId docID);
extern "C" COREDLL_API void redo(void* pObject, DocumentId docID);

extern "C" COREDLL_API void wheel(void* pObject, DocumentId docID, int val);
extern "C" COREDLL_API void draw(void* pObject, DocumentId docID);
extern "C" COREDLL_API void activateDocument(void* pObject, DocumentId docID, int w, int h);
extern "C" COREDLL_API void resizeDocument(void* pObject, DocumentId docID, int w, int h);

//document factory method
extern "C" COREDLL_API void* documentFactory(void* hwnd);

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

#pragma region test

#pragma pack(push, 1)
struct CallbackValues
{
public:
	double thickness = 0;
	int size = 0;
	char* line = nullptr;
	int flag = 0;
	char* pString = nullptr;
	int* pInt = nullptr;
	double* pDouble = nullptr;
};
#pragma pack(pop)

typedef void(COREDLL_CALL *callBackFunction)(CallbackValues);

extern "C" COREDLL_API void TestPInvoke(callBackFunction f);

#pragma endregion test