#pragma once
#include "headers.h"
#include "session.h"
#include "connect_data.h"
using namespace std;

//иерархия операций
//операции делятся на два типа: 
//	1) которые пишутся в историю модели, для них выполняется механизм undo/redo
//		a) самостоятельные
//		б) зависимые от других операций (от одной или нескольких)
//	2) которые не пишутся в дерево модели
//		а) команды undo/redo
//		б) команды редактирования дерева модели
//		с) сервисные команды

class Operation
{
public:
	//подготовка данных в контроллере
	virtual void operator()(Session& curSes, DOCID doc) = 0;

	//отправка подготовленных данных для исполнения в ядро
	void messageToKern(Session& curSes, DOCID doc, ConnectData& source, FEATID featureID)
	{
		curSes.getDocument(doc)->addNodeToModelTree(source, featureID);
	}
};

//ПЕРВЫЙ ТИП ОПЕРАЦИЙ

//создание точки
class createPointOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//создание линии
class createLineOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//создание окружности
class createCircleOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//создание дуги
class createArcOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//создание полилинии
class createPolyLineOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//редактирование линии
class setLinePointOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//разметка геометрии конечными элементами (создание сетки)
class createMeshOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//задание закреплений
class createSupportOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//задание нагрузок
class createLoadOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//ВТОРОЙ ТИП ОПЕРАЦИЙ

//измерение расстояния между двумя точками
class measureDistanceOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//измерение толщины геометрического примитива
class changeThicknessOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//измерение цвета геометрического примитива
class changeColorOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};

//откат назад
class undoOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc) { curSes.getDocument(doc)->undo(); }
};

//повтор откатаной операции
class redoOperation: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc) { curSes.getDocument(doc)->redo(); }
};

//изменение кеша undo/redo
class changeCacheUndoRedo: public Operation
{
	public:
	void operator()(Session& curSes, DOCID doc);
};

//выбор слоя/слоев для отображения
class selectLayers: public Operation
{
public:
	void operator()(Session& curSes, DOCID doc);
};