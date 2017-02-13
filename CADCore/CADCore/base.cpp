#include "base.h"

void Base::notify(void)
{
	_observer->update(_base);
}

ObjectId Base::attachToBase(Generic* object)
{
	++_counter;
	_base[_counter] = object;
	notify();
	return _counter;
}

Generic* Base::detachFromBase(ObjectId objID)
{
	auto iter = _base.find(objID);
	
	if(iter != _base.end())
	{
		Generic* temp = getGeneric(objID);
		_base.erase(iter);
		notify();
		return temp;
	}
	else
	{
		return nullptr;
	}
}

Generic* Base::getGeneric(ObjectId objID)
{
	auto iter = _base.find(objID);
	
	if(iter != _base.end())
		return iter->second;
	else
		return nullptr;
}

Topology* Base::getGenericTopology(ObjectId objID)
{
	return getGeneric(objID)->getTopology();
}

void Base::commit(void)
{
	_history.commit(_base);
}

void Base::undo(void)
{
	_history.undo(_base);
	notify();
}

void Base::redo(void)
{
	_history.redo(_base);
	notify();
}

void Buffer::update(const std::map<ObjectId, Generic*>& baseState)
{
	auto iter = std::remove_if(_buffer.begin(), _buffer.end(),
		[](std::pair<ObjectId, Generic*> curPair)->bool
	{
		if(curPair.first != NOT_FROM_BASE)
			return true;
		else
			return false;
	});

	_buffer.erase(iter, _buffer.end());
	
	std::for_each(baseState.begin(), baseState.end(), 
		[&](std::pair<ObjectId, Generic*> curPair)
	{
		_buffer.push_back(curPair);
	});
}

void Buffer::attachToBuffer(Generic* object)
{
	_buffer.push_back(std::make_pair(NOT_FROM_BASE, object)); 
}

void Buffer::detachFrombuffer(Generic* object)
{
	auto iter = std::find(_buffer.begin(), _buffer.end(), std::make_pair(NOT_FROM_BASE, object));
	
	if(iter != _buffer.end())
		_buffer.erase(iter);
	else
		return;
}

void Buffer::toScreen(void)
{
#ifdef TRACE_DEBUG
	std::cout << "ON THE SCREEN:" << std::endl << std::endl;
#endif
	std::for_each(_buffer.begin(), _buffer.end(),
		[=](std::pair<ObjectId, Generic*> curPair)
	{
		auto iter = std::find(_layers.begin(), _layers.end(), curPair.second->getLayer());
		
		if( iter == _layers.end() )
		{
			return;
		}
		else
		{
#ifdef TRACE_DEBUG
			std::cout << "ID: " << curPair.first << ". ";
#endif
			curPair.second->getTopology()->drawing();
#ifdef TRACE_DEBUG
			std::cout << "Color: " << curPair.second->getColor();
			std::cout << ", Thickness: " << curPair.second->getThickness();
			std::cout << ", Layer: " << curPair.second->getLayer();
#endif
			
			bool whereObject = true;
			if(curPair.first == NOT_FROM_BASE)
				whereObject = false;
			
#ifdef TRACE_DEBUG
			std::cout << ", From controller (0) or base (1): " << whereObject << ".";
			std::cout << std::endl << std::endl;
#endif
		}
	});

#ifdef TRACE_DEBUG
	std::cout << std::endl;
#endif
}
