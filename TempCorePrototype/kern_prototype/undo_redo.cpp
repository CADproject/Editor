#include "headers.h"
#include "undo_redo.h"
using namespace std;

void UndoRedo::addEvent(Node* newNode)
{
	//удаление generic'ов безвозвратно
	if( (!_container.empty()) && (_counter != _container.size()-1) )
	{
		for(size_t i = _counter+1; i < _container.size(); i++)
			getMyTree()->deleteNode(_container.at(i));

		assert(_counter == _container.size()-1);
	}
		
	if(_container.size() < _size)
	{
		_container.push_back(newNode);
	}
	else
	{
		_container.pop_front();
		_container.push_back(newNode);
	}
	_counter = _container.size()-1;

	assert(_container.size() == _size);
}

void UndoRedo::undo(void)
{
	if(_counter == 0) return;

	for(size_t i = 0; i < _container.at(_counter)->getResultIDs().size(); i++)
		getMyBase()->detachObject( _container.at(_counter)->getResultIDs().at(i) );
						
	--_counter;
}

void UndoRedo::redo(void)
{
	if(_counter == _container.size()-1) return;

	_container.at(_counter)->runFeature( *getMyBase() );

	++_counter;
}

void UndoRedo::clear(void)
{
	//удаление generic'ов безвозвратно
	if( (!_container.empty()) && (_counter != _container.size()-1) )
	{
		for(size_t i = _counter+1; i < _container.size(); i++)
			getMyTree()->deleteNode(_container.at(i));

		assert(_counter == _container.size()-1);
	}
				
	_container.clear();
	_counter = 0;
}