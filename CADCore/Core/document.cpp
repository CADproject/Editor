#include <algorithm>
#include <iostream>
#include "document.h"

void Buffer::attachToBuffer(Generic* object)
{
	_buffer.push_back(object);
}

void Buffer::detachFrombuffer(Generic* object)
{
	auto iter = std::find(_buffer.begin(), _buffer.end(), object);

	if(iter != _buffer.end())
		_buffer.erase(iter);
	else
		return;
}

void Buffer::toScreen(void)
{
	std::cout << "ON THE SCREEN:" << std::endl;
	
	std::for_each(_buffer.begin(), _buffer.end(),
		[=](Generic* object)
	{
		auto iter = std::find(_layers.begin(), _layers.end(), object->getLayer());
		
		if( iter == _layers.end() )
		{
			return;
		}
		else
		{
			std::cout << "Topology: " << object->getTopology();
			std::cout << ", Color: " << object->getColor();
			std::cout << ", Thickness: " << object->getThickness();
			std::cout << ", Layer: " << object->getLayer();
			std::cout << std::endl;
		}
	});

	std::cout << std::endl;
}