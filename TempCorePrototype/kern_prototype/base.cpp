#include "headers.h"
#include "base.h"
using namespace std;

void Buffer::update(vector<genericObject*>& uptodate) 
{ 
	_buffer = uptodate;
	toScreen();
}

void Buffer::toScreen(void)
{
	//нужно стереть то, что было на экране до этого,
	//но в консоли это не имеет смысла

	for(size_t i = 0; i < _buffer.size(); i++)
	{
		for(size_t j = 0; j < _layers.size(); j++)
		{
			if( _buffer.at(i)->getLayer() == _layers.at(j) )
			{
				_buffer.at(i)->show();
				break;
			}
		}
	}
}

void Base::attachToBase(pair<Geometry*, OBJID>& newPair)
{
	genericObject* newObject = new genericObject(newPair.first);
	assert(_base.insert(make_pair(newPair.second, newObject)).second);
	notify();
}

void Base::detachObject(OBJID id)
{
	_base.erase(id);
	notify();
}

void Base::clearBase(void) 
{ 
	_base.clear();
	notify();
}

genericObject* Base::getGenericObject(OBJID id)
{
	auto iter = _base.find(id);

	if(iter != _base.end())
		return (*iter).second;
	else
		return nullptr;
}

vector<genericObject*> Base::getGenericObjects(void)
{
	vector<genericObject*> pObjects;

	for(auto iter = _base.begin(); iter != _base.end(); ++iter)
		pObjects.push_back( (*iter).second );

	return pObjects;
}

void Base::notify(void) 
{ 
	vector<genericObject*> pObjects = getGenericObjects();
	_observer->update(pObjects); 
}