#pragma once
#include "headers.h"
#include "model_tree.h"
#include "base.h"
#include "undo_redo.h"
using namespace std;

//основная абстракция приложения
//содержит в себе информацию, которая пишется в файл

class Document
{
private:
	static DOCID _id;		//идентификатор документа (для текущей сессии)
	ModelTree _tree;		//структура данных модели
	Buffer _buffer;			//фильтрует объекты для вывода на экран
	UndoRedo _undoredo;		//список undo/redo

public:
	Document() { _tree.getBase().attachObserver(&_buffer); }
	~Document() {}

	//API интерфейс ядра

	void addNodeToModelTree(ConnectData& source, FEATID featureID);

	void selectLayers(vector<unsigned>& layersToShow);

	void changeThickness(OBJID id, THICKNESS newThickness);

	void changeColor(OBJID id, COLOR newColor);

	void undo(void);

	void redo(void);

	void changeCacheUndoRedo(unsigned cache);

	PointStruct getPointCoords(OBJID id);
};