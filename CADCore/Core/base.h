/*This file contains the base class*/

#pragma once
#include <map>
#include "topology.h"
#include "definitious.h"

class Generic	//generalized base object
{
	unsigned _layer;		//the number of layer in which the primitive is
	COLOR _color;			//the color of the primitive
	THICKNESS _thickness;	//the thickness of the primitive
	Topology* _primitive;	//the type of the primitive
};

class Buffer	//contains all objects which are drawn on the screen
{
private:
	std::vector<Generic*> _buffer;	//the objects that appear on the screen
	std::vector<unsigned> _layers;	//the layers that appear on the screen

public:
	void attachToBuffer(Generic* object);
};

class Base
{
private:
	std::map<OBJID, Generic*> _base;	//the objects are written to file

public:
	Base() {}
	~Base() {}

	OBJID attachToBase(Generic* object);
	void detachFromBase(OBJID id);
	Generic* getGeneric(OBJID id);
};
