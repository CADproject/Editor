/*This file contains the undo/redo mechanism. The container undo/redo
keeps a history of last N commands in order of execution.
Used pattern Memento from GOF catalogue.*/

#pragma once
#include <deque>
#include <map>
#include "definitions.h"
#include "base.h"

class UndoRedo
{
private:
	std::deque< std::map<OBJID, Generic*> > _snapshots;
	unsigned _size;
	unsigned _counter;

public:
	UndoRedo(): _size(0), _counter(0) {}
	~UndoRedo() {}

	void setSize(unsigned newSize) { _size = newSize; }

	void commit(void);
	void undo(void);
	void redo(void);
	void clear(void);
};