#include <algorithm>
#include <iostream>
#include "base.h"

unsigned Base::_counter;

void Base::notify(void)
{
	_observer->update(_base);
}

OBJID Base::attachToBase(Generic* object)
{
	++_counter;
	_base[_counter] = object;
	notify();
	return _counter;
}

Generic* Base::detachFromBase(OBJID objID)
{
	auto iter = _base.find(objID);
	
	if(iter != _base.end())
	{
		_base.erase(iter);
		notify();
		return getGeneric(objID);
	}
	else
	{
		return nullptr;
	}
}

Generic* Base::getGeneric(OBJID objID)
{
	auto iter = _base.find(objID);
	
	if(iter != _base.end())
		return iter->second;
	else
		return nullptr;
}

void Base::commit(void)
{
	_undoredo.commit(_base);
}

void Base::undo(void)
{
	_undoredo.undo(_base);
	notify();
}

void Base::redo(void)
{
	_undoredo.redo(_base);
	notify();
}

void Buffer::update(const std::map<OBJID, Generic*>& baseState)
{
	auto iter = std::remove_if(_buffer.begin(), _buffer.end(),
		[](std::pair<bool, Generic*> curPair)->bool
	{
		if(curPair.first)
			return true;
		else
			return false;
	});

	_buffer.erase(iter, _buffer.end());

	std::for_each(baseState.begin(), baseState.end(), 
		[=](std::pair<OBJID, Generic*> curPair)
	{
		_buffer.push_back(std::make_pair(true, curPair.second));
	});
}

void Buffer::attachToBuffer(Generic* object, bool fromBase)
{
	_buffer.push_back(std::make_pair(fromBase, object)); 
}

void Buffer::detachFrombuffer(Generic* object, bool fromBase)
{
	auto iter = std::find(_buffer.begin(), _buffer.end(), std::make_pair(fromBase, object));

	if(iter != _buffer.end())
		_buffer.erase(iter);
	else
		return;
}

void Buffer::toScreen(void)
{
	std::cout << "ON THE SCREEN:" << std::endl;
	
	std::for_each(_buffer.begin(), _buffer.end(),
		[=](std::pair<bool, Generic*> curPair)
	{
		auto iter = std::find(_layers.begin(), _layers.end(), curPair.second->getLayer());
		
		if( iter == _layers.end() )
		{
			return;
		}
		else
		{
			std::cout << "Topology: " << curPair.second->getTopology();
			std::cout << ", Color: " << curPair.second->getColor();
			std::cout << ", Thickness: " << curPair.second->getThickness();
			std::cout << ", Layer: " << curPair.second->getLayer();
			std::cout << ", From controller (0) or from base (1): " << curPair.first;
			std::cout << std::endl;
		}
	});

	std::cout << std::endl;
}