/*This file contains the base class, generic class, undo/redo mechanism, buffer class.
The container undo/redo keeps a history of last N commands in order of execution.
Used patterns Memento and Observer from GOF catalogue.*/

#pragma once
#include "headers.h"

class Generic	//generalized base object, must be created using operator "new"
{
private:
	unsigned _layer;		//the number of layer in which the primitive is
	COLOR _color;			//the color of the primitive
	THICKNESS _thickness;	//the thickness of the primitive
	Topology* _primitive;	//the type of the primitive
	__int64 _id;
	static __int64 _counter;

	void Init()
	{
		_id = _counter++;
	}
public:
	Generic() : _layer(0), _color(BLACK), _thickness(THREE), _primitive(nullptr) { Init(); }
	Generic(Topology* primitive) : _layer(0), _color(BLACK), _thickness(THREE), _primitive(primitive) { Init(); }
	Generic(unsigned layer, Topology* primitive) : _layer(layer), _color(BLACK), _thickness(THREE), _primitive(primitive) { Init(); }
	Generic(unsigned layer, COLOR color, THICKNESS thickness, Topology* primitive) :
		_layer(layer), _color(color), _thickness(thickness), _primitive(primitive) { Init(); }

	unsigned getLayer(void) { return _layer; }
	COLOR getColor(void) { return _color; }
	THICKNESS getThickness(void) { return _thickness; }
	Topology* getTopology(void) { return _primitive; }

	void setLayer(unsigned newLayer) { _layer = newLayer; }
	void setColor(COLOR newColor) { _color = newColor; }
	void setThickness(THICKNESS newThickness) { _thickness = newThickness; }
	void setTopology(Topology* newPrimitive) { _primitive = newPrimitive; }
	__int64 GetId() { return _id; }
};

class History
{
private:
	std::deque< std::map<ObjectId, Generic*> > _snapshots;
	unsigned _size;
	unsigned _counter;

public:
	History(): _size(10), _counter(0) { _snapshots.push_back(std::map<ObjectId, Generic*>()); }
	~History() {}

	void setSize(unsigned newSize) { _size = newSize; }

	void commit(const std::map<ObjectId, Generic*>& curBase);
	void undo(std::map<ObjectId, Generic*>& curBase);
	void redo(std::map<ObjectId, Generic*>& curBase);
	void clear(void);
};

class Buffer	//contains all objects which are drawn on the screen
{
private:
	std::vector< std::pair<ObjectId, Generic*> > _buffer;	//the objects that appear on the screen
														//if OBJID == 0 then object from controller, else from base
	std::vector<unsigned> _layers;						//the layers that appear on the screen

public:
	Buffer() { _layers.push_back(0); }

	void update(const std::map<ObjectId, Generic*>& baseState);

	void setLayers(std::vector<unsigned>& newLayers) { _layers = newLayers; }
	std::vector<unsigned> getLayers(void) { return _layers; }
	
	void attachToBuffer(Generic* object);		//to display generic on the screen
	void detachFrombuffer(Generic* object);		//to remove generic from the screen

	void toScreen(void);
};

class Base
{
private:
	unsigned _counter;					//generics counter
	std::map<ObjectId, Generic*> _base;	//the objects are written to file
	History _history;					//undo/redo mechanism
	Buffer* _observer;					//pattern Observer from GOF catalogue

public:
	Base(): _counter(0), _observer(nullptr) {}

	void notify(void);
	void attachObserver(Buffer* observer) { _observer = observer; }

	ObjectId attachToBase(Generic* object);
	Generic* detachFromBase(ObjectId objID);
	Generic* getGeneric(ObjectId objID);

	Topology* getGenericTopology(ObjectId objID);

	void commit(void);
	void undo(void);
	void redo(void);
};