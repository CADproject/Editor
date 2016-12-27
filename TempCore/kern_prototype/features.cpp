#include "headers.h"
#include "features.h"
using namespace std;

bool createPointFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	PointStruct point = source.getPoints().at(0);	
	Point* newPoint = new Point(point);
	OBJID recoveryID = result.getIDs().at(0);
		
	pair<Geometry*, OBJID> newPair = make_pair(newPoint, recoveryID);
	base.attachToBase(newPair);
		
	return true;
}

bool createLineFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	PointStruct start = source.getPoints().at(0);
	PointStruct end = source.getPoints().at(1);
	Line* newLine = new Line(start, end);
	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newLine, recoveryID);
	base.attachToBase(newPair);

	return true;
}

bool createCircleFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	PointStruct center = source.getPoints().at(0);
	PointStruct side = source.getPoints().at(1);	
	Circle* newCircle = new Circle(center, side);
	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newCircle, recoveryID);
	base.attachToBase(newPair);
	
	return true;
}

bool createArcFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	PointStruct center = source.getPoints().at(0);
	PointStruct start = source.getPoints().at(1);
	double angle = source.getDnums().at(0);		
	Arc* newArc = new Arc(center, start, angle);
	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newArc, recoveryID);
	base.attachToBase(newPair);
	
	return true;
}

bool createPolylineFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	vector<PointStruct> points = source.getPoints();
	PolyLine* newPolyLine = new PolyLine(points);
	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newPolyLine, recoveryID);		
	base.attachToBase(newPair);

	return true;
}

bool setLinePointFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	OBJID lineID = source.getIDs().at(0);
	
	//если в базе нет такого элемента, то просто ничего не делаем
	if( !base.getGenericObject(lineID) ) return false;
	
	Line* lineObj = dynamic_cast<Line*>( base.getGenericObject(lineID)->getGeometry() );

	PointStruct startPoint = lineObj->getStart();
	PointStruct newEndPoint = source.getPoints().at(0);
	
	//удаляем старый объект
	base.detachObject(lineID);

	//создаем новую кривую того же типа по новым параметрам
	Line* newLine = new Line(startPoint, newEndPoint);
	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newLine, recoveryID);	
	base.attachToBase(newPair);
	
	return true;
}

bool createMeshFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	OBJID geometryID = source.getIDs().at(0);
	
	//проверяем есть ли в базе элементы с такими id,
	//если нету, то просто ничего не делаем
	if( !base.getGenericObject(geometryID) ) return false;

	int numberFE = source.getInums().at(0);

	Geometry* geometry = base.getGenericObject(geometryID)->getGeometry();
	
	Mesh* newMesh = new Mesh(geometry, numberFE);
	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newMesh, recoveryID);		
	base.attachToBase(newPair);

	return true;
}

bool createSupportFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	OBJID meshID = source.getIDs().at(0);

	//проверяем есть ли в базе элементы с такими id,
	//если нету, то просто ничего не делаем
	if( !base.getGenericObject(meshID) ) return false;

	int dof = source.getInums().at(0);
	
	Geometry* geometry = base.getGenericObject(meshID)->getGeometry();
	Mesh* mesh = dynamic_cast<Mesh*>(geometry);

	Support* newSupport = new Support(mesh, (DOF)dof);

	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newSupport, recoveryID);
	base.attachToBase(newPair);
	
	return true;
}

bool createLoadFeat::runFeat(ConnectData& source, ConnectData& result, Base base)
{
	OBJID meshID = source.getIDs().at(0);

	//проверяем есть ли в базе элементы с такими id,
	//если нету, то просто ничего не делаем
	if( !base.getGenericObject(meshID) ) return false;

	int dof = source.getInums().at(0);
	double value = source.getDnums().at(0);
	
	Geometry* geometry = base.getGenericObject(meshID)->getGeometry();
	Mesh* mesh = dynamic_cast<Mesh*>(geometry);

	Load* newLoad = new Load(mesh, (DOF)dof, value);

	OBJID recoveryID = result.getIDs().at(0);
	
	pair<Geometry*, OBJID> newPair = make_pair(newLoad, recoveryID);
	base.attachToBase(newPair);
	
	return true;
}