/*This file contains the base class*/

#pragma once
#include "topology.h"
#include "definitious.h"

class Generic	//generalized base object
{
	unsigned _layer;		//the number of layer in which the primitive is
	COLOR _color;			//the color of the primitive
	THICKNESS _thickness;	//the thickness of the primitive
	Topology* _primitive;	//the type of the primitive
};

class Base
{

};
