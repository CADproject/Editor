#include <cassert>
#include "undo_redo.h"

void UndoRedo::commit(void)
{
	if( !_snapshots.empty() && _counter != _snapshots.size()-1 )
	{
		while(_counter < _snapshots.size()-1)
			_snapshots.pop_back();
		
		assert(_counter == _snapshots.size()-1);
	}

	//снимок базы

	if(_snapshots.size() < _size)
	{
		_snapshots.push_back(/*new shapshot*/);
		++_counter;
	}
	else
	{
		_snapshots.pop_front();
		_snapshots.push_back(/*new shapshot*/);
	}

	assert(_snapshots.size() <= _size);
}

void UndoRedo::undo(void)
{
	if(_counter == 0)
		return;

	--_counter;

	//связь с базой - перезапись нового снимка "_snapshots.at(_counter)" в базу
}

void UndoRedo::redo(void)
{
	if(_counter == _snapshots.size()-1)
		return;

	++_counter;

	//связь с базой - перезапись нового снимка "_snapshots.at(_counter)" в базу
}

void UndoRedo::clear(void)
{
	_snapshots.clear();
	_counter = 0;
}