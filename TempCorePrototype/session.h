#pragma once
#include "headers.h"
#include "document.h"
using namespace std;

//объект класса Session существует в единственном экземпл€ре
//его основна€ работа - регистрировать документы (Document) в каждом сеансе
//работы во врем€ выполнени€ и присваивать им идентификаторы

class Session
{
private:
	static unsigned counter;
	map<DOCID, Document*> _session;
	
public:
	Session() {}
	~Session() {}

	//зарегистрировать документ в текущей сессии
	DOCID registerDocument(Document* pdoc)
	{
		counter++;
		_session[counter] = pdoc;
		return counter;
	}

	//удалить документ из текущей сессии
	void removeDocument(DOCID doc) { _session.erase(doc); }

	//получить документ по идентификатору
	Document* getDocument(DOCID doc)
	{
		auto iter = _session.find(doc);

		if(iter == _session.end())
			return nullptr;
		else
			return iter->second;
	}
};