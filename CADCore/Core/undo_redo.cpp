#include <cassert>
#include "base.h"

void UndoRedo::commit(const std::map<OBJID, Generic*>& curBase)
{
	if( !_snapshots.empty() && _counter != _snapshots.size()-1 )
	{
		while(static_cast<size_t>(_counter) < _snapshots.size()-1)
			_snapshots.pop_back();
		
		assert(_counter == _snapshots.size()-1);
	}

	if(_snapshots.size() < _size)
	{
		_snapshots.push_back(curBase);
		++_counter;
	}
	else
	{
		_snapshots.pop_front();
		_snapshots.push_back(curBase);
	}

	assert(_snapshots.size() <= _size);
}

void UndoRedo::undo(std::map<OBJID, Generic*>& curBase)
{
	if(_counter == 0)
		return;

	--_counter;

	curBase = _snapshots.at(_counter);
}

void UndoRedo::redo(std::map<OBJID, Generic*>& curBase)
{
	if(_counter == _snapshots.size()-1)
		return;

	++_counter;

	curBase = _snapshots.at(_counter);
}

void UndoRedo::clear(void)
{
	_snapshots.clear();
	_counter = -1;
}