#pragma once
#include "headers.h"
#include "base.h"
#include "features.h"
#include "connect_data.h"
using namespace std;

//узел дерева модели
//по исходным данным может воспроизводить объекты базы

class Node
{
private:
	ConnectData _source;		//исходные данные для фичи
	FEATID _featureId;			//идентификатор фичи
	ConnectData _result;		//результат работы фичи
	vector<Node*> _parents;		//предки, их может быть много
	vector<Node*> _children;	//потомки

public:
	Node() { _featureId = ABSTRUCT_FEATURE; }

	Node(const ConnectData& source, FEATID featureId, const ConnectData& result)
	{
		_source = source;
		_featureId = featureId;
		_result = result;
	}
	
	~Node() {}

	//добавление дочерних узлов 
	void AddChildren(Node* pNode) { _children.push_back(pNode); }
	
	//доступ к списку дочерних узлов
	vector<Node*> getChildren(void) { return _children; }

	//возвращает список предков
	vector<Node*> getParents(void) { return _parents; }

	//возвращает id результатов фичи
	vector<OBJID> getResultIDs(void) { return _result.getIDs(); }
	
	Feature* createFeature(void);

	//запуск фичи конкретного узла
	//если возвращает false, то рекурсию в этом направлении надо прервать
	bool runFeature(Base& base);
};

//структура данных модели			//base желательно вытащить из ModelTree!!

class ModelTree
{
private:
	static unsigned _counter;	//счетчик OBJID (для назначения новых)
	Base _base;					//база модели
	Node* _root;				//корень дерева модели
public:
	ModelTree() { _root = new Node(); }
	~ModelTree() {}

	//temp
	Base getBase(void) { return _base; }

	//выполнение дерева (или поддерева)
	void runTree(Node* pNode);

	//добавление узла
	void addNode(ConnectData& source, FEATID featureID);

	//удаление узла (и его поддерева, если есть)
	void deleteNode(Node* pNode);

	//поиск узла по результатам фичи в дереве (поддереве)
	Node* searchNode(Node* startNode, OBJID id);

	//технический метод отображения дерева
	void showTree(void)
	{
		//todo
	}
};