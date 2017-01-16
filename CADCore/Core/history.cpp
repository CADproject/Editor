#include "base.h"

void History::commit(const std::map<ObjectId, Generic*>& curBase)
{
	if( !_snapshots.empty() && _counter != _snapshots.size()-1 )
	{
		while(_counter < _snapshots.size()-1)
		{
			std::map<ObjectId, Generic*> candidates = _snapshots.back();
			_snapshots.pop_back();

			std::for_each(candidates.begin(), candidates.end(),
				[=](std::pair<ObjectId, Generic*> cand)
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
		std::map<ObjectId, Generic*> candidates = _snapshots.front();
		
		_snapshots.pop_front();
		_snapshots.push_back(curBase);

		std::for_each(candidates.begin(), candidates.end(),
			[=](std::pair<ObjectId, Generic*> cand)
		{
			auto result = _snapshots.front().find(cand.first);
							
			if(result == _snapshots.front().end())
				delete cand.second;
		});
	}

	assert(_snapshots.size() <= _size);
}

void History::undo(std::map<ObjectId, Generic*>& curBase)
{
	if(_counter == 0)
		return;

	--_counter;

	curBase = _snapshots.at(_counter);
}

void History::redo(std::map<ObjectId, Generic*>& curBase)
{
	if(_counter == _snapshots.size()-1)
		return;

	++_counter;

	curBase = _snapshots.at(_counter);
}

void History::clear(void)
{
	_snapshots.erase(_snapshots.begin()+1, _snapshots.end());
	_counter = 0;
}