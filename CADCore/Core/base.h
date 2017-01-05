/*This file contains the base class, generic class, undo/redo mechanism.
The container undo/redo keeps a history of last N commands in order of execution.
Used pattern Memento from GOF catalogue.*/

#pragma once
#include <map>
#include <deque>
#include "definitions.h"
#include "topology.h"

class Generic	//generalized base object
{
private:
	unsigned _layer;		//the number of layer in which the primitive is
	COLOR _color;			//the color of the primitive
	THICKNESS _thickness;	//the thickness of the primitive
	Topology* _primitive;	//the type of the primitive

public:
	Generic(): _layer(0), _color(BLACK), _thickness(THREE), _primitive(nullptr) {}
	Generic(Topology* primitive): _layer(0), _color(BLACK), _thickness(THREE), _primitive(primitive) {}
	Generic(unsigned layer, COLOR color, THICKNESS thickness, Topology* primitive):
		_layer(layer), _color(color), _thickness(thickness), _primitive(primitive) {}

	unsigned getLayer(void) { return _layer; }
	COLOR getColor(void) { return _color; }
	THICKNESS getThickness(void) { return _thickness; }
	Topology* getTopology(void) { return _primitive; }

	void setLayer(unsigned newLayer) { _layer = newLayer; }
	void setColor(COLOR newColor) { _color = newColor; }
	void setThickness(THICKNESS newThickness) { _thickness = newThickness; }
	void setTopology(Topology* newPrimitive) { _primitive = newPrimitive; }
};

class UndoRedo
{
private:
	std::deque< std::map<OBJID, Generic*> > _snapshots;
	unsigned _size;
	unsigned _counter;

public:
	UndoRedo(): _size(10), _counter(0) {}
	~UndoRedo() {}

	void setSize(unsigned newSize) { _size = newSize; }

	void commit(const std::map<OBJID, Generic*>& curBase);
	void undo(std::map<OBJID, Generic*>& curBase);
	void redo(std::map<OBJID, Generic*>& curBase);
	void clear(void);
};

class Base
{
private:
	std::map<OBJID, Generic*> _base;	//the objects are written to file
	UndoRedo _undoredo;					//undo/redo mechanism

public:
	Base() {}
	~Base() {}

	OBJID attachToBase(Generic* object);
	void detachFromBase(OBJID id);
	Generic* getGeneric(OBJID id);

	void commit(void);
	void undo(void);
	void redo(void);
};
