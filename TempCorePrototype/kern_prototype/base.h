#pragma once
#include "headers.h"
#include "geometry.h"
using namespace std;

//обобщенный объект базы (объект-контейнер)
//базовый класс для всех абстракций модели
//(СК, геометрический примитив, КЭ, нагрузка, закрепление и т.д.)

class genericObject
{
private:
	unsigned _layer;		//номер слоя элемента
	COLOR _color;			//цвет геометрического элемента
	THICKNESS _thickness;	//толщина геометрического элемента
	Geometry* _geometry;	//хранимый объект

public:
	genericObject() {}
	genericObject(Geometry* geometry) { _geometry = geometry; }
	~genericObject() {}

	unsigned getLayer(void) { return _layer; }
	Geometry* getGeometry(void) { return _geometry; }

	void setColor(COLOR newColor) { _color = newColor; }
	void setThickness(THICKNESS newThickness) { _thickness = newThickness; }

	void show(void) { _geometry->show(); }
};

//промежуточный слой между базой модели и экраном
//нужен для фильтрации и управления отображаемыми объектами

class Buffer
{
private:
	vector<genericObject*> _buffer;		//обобщенные объекты модели
	vector<unsigned> _layers;			//отображаемые на экране слои
public:
	Buffer() { _layers.push_back(0); }
	~Buffer() {}

	void setLayers(vector<unsigned>& layersToShow) { _layers = layersToShow; }

	//обновление буффера (от издателя)
	void update(vector<genericObject*>& uptodate);
	
	//фильтрация по слоям и вывод на экран
	void toScreen(void);
};

//база модели содержит идентификаторы всех существующих на данный момент объектов

class Base
{
private:
	map<OBJID, genericObject*> _base;	//база модели
	Buffer* _observer;					//подписчик базы
public:
	Base() {}
	~Base() {}

	//прикрепление подписчика
	void attachObserver(Buffer* observer) { _observer = observer; }

	//прикрепление нового обобщенного объекта
	void attachToBase(pair<Geometry*, OBJID>& newPair);

	//открепить объект от базы
	void detachObject(OBJID id);

	//очистить базу
	void clearBase(void);
	
	//получение указателя на обобщенный объект базы
	genericObject* getGenericObject(OBJID id);

	//получить generic'и
	vector<genericObject*> getGenericObjects(void);

	//уведомление подписчика
	void notify(void);
};