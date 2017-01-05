/*This file contains the base class, generic class, buffer class, undo/redo mechanism.
The container undo/redo keeps a history of last N commands in order of execution.
Used pattern Memento from GOF catalogue.*/

#pragma once
#include <map>
#include <deque>
#include "definitions.h"
#include "topology.h"

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
