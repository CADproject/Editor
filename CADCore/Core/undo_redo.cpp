#include <cassert>
#include <algorithm>
#include "base.h"

void UndoRedo::commit(const std::map<OBJID, Generic*>& curBase)
{
	if( !_snapshots.empty() && _counter != _snapshots.size()-1 )
	{
		while(static_cast<size_t>(_counter) < _snapshots.size()-1)
		{
			std::map<OBJID, Generic*> candidates = _snapshots.back();
			_snapshots.pop_back();

			std::for_each(candidates.begin(), candidates.end(),
				[=](std::pair<OBJID, Generic*> cand)
			{
				auto result = _snapshots.back().find(cand.first);
							
				if(result == _snapshots.back().end())
					delete cand.second;
			});
		}

		assert(_counter == _snapshots.size()-1);
	}

	if(_snapshots.size() < _size)
	{
		_snapshots.push_back(curBase);
		++_counter;
	}
	else
	{
		std::map<OBJID, Generic*> candidates = _snapshots.front();
		
		_snapshots.pop_front();
		_snapshots.push_back(curBase);

		std::for_each(candidates.begin(), candidates.end(),
			[=](std::pair<OBJID, Generic*> cand)
		{
			auto result = _snapshots.front().find(cand.first);
							
			if(result == _snapshots.front().end())
				delete cand.second;
		});
	}

	assert(_snapshots.size() <= _size);
}

void UndoRedo::undo(std::map<OBJID, Generic*>& curBase)
{
	if(_counter == 0)	//-1
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