#pragma once
#include "headers.h"
#include "model_tree.h"
using namespace std;

//контейнер undo/redo хранит историю последних N команд в порядке выполнения
//с помощью этого класса можно удалять объекты из базы модели (с возможностью восстановления)
//или из структуры данных модели (безвозвратно)

class UndoRedo
{
private:
	deque<Node*> _container;	//кеш undo/redo
	size_t _size;				//размер кеша undo/redo
	size_t _counter;			//текущее положение в кеше

public:
	UndoRedo() { _size = 10; _counter = 0; }
	~UndoRedo() {}

	//temp
	ModelTree* getMyTree(void) { return new ModelTree(); }
	//temp
	Base* getMyBase(void) { return new Base(); }

	//задать размер кеша
	void setSize(size_t newSize) { _size = newSize; }

	//запись в кеш
	void addEvent(Node* newNode);

	//откат операции
	void undo(void);

	//восстановление операции
	void redo(void);
	
	//очистка кеша
	void clear(void);
};