#pragma once
#include "headers.h"
#include "session.h"

#ifdef COREDLL_EXPORT
	#define COREDLL_API __declspec(dllexport)
#else
	#define COREDLL_API __declspec(dllimport)
#endif

//session factory method
extern "C" COREDLL_API Session* sessionFactory(void);

//document factory method
extern "C" COREDLL_API Document* documentFactory(void);

//node factory method
extern "C" COREDLL_API Node* nodeFactory(double x, double y);

//point factory method
extern "C" COREDLL_API Point* pointFactory(const Node& node);

extern "C" COREDLL_API Line* lineFactory(const Node& start, const Node& end);

//circle factory method
extern "C" COREDLL_API Circle* circleFactory(const Node& center, const Node& side);

//contour factory method
extern "C" COREDLL_API Contour* contourFactory(const std::vector<Edge*>& edges);

//generic factory method
extern "C" COREDLL_API Generic* genericFactory(Topology* primitive, unsigned layer = 0, COLOR color = BLACK, THICKNESS thickness = THREE);