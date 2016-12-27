#pragma once
#include "headers.h"
#include "geometry.h"
#include "base.h"
#include "connect_data.h"
using namespace std;

//иерархия фич
//фича - основная часть операции, исключающая все подготовительные действия
//при выполнении фич база модели восстанавливается по стуктуре данных модели

class Feature
{
public:
	Feature() {}
	virtual ~Feature() {}

	//выполнение фичи
	virtual bool runFeat(ConnectData& source, ConnectData& result, Base base) = 0;
};

class createPointFeat: public Feature
{
public:
	createPointFeat() {}
	~createPointFeat() {}

	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createLineFeat: public Feature
{
public:
	createLineFeat() {}
	~createLineFeat() {}

	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createCircleFeat: public Feature
{
public:
	createCircleFeat() {}
	~createCircleFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createArcFeat: public Feature
{
public:
	createArcFeat() {}
	~createArcFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createPolylineFeat: public Feature
{
public:
	createPolylineFeat() {}
	~createPolylineFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class setLinePointFeat: public Feature
{
public:
	setLinePointFeat() {}
	~setLinePointFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createMeshFeat: public Feature
{
public:
	createMeshFeat() {}
	~createMeshFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createSupportFeat: public Feature
{
public:
	createSupportFeat() {}
	~createSupportFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};

class createLoadFeat: public Feature
{
public:
	createLoadFeat() {}
	~createLoadFeat() {}
	
	bool runFeat(ConnectData& source, ConnectData& result, Base base);
};