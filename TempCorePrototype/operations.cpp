#include "headers.h"
#include "operations.h"
using namespace std;

void createPointOperation::operator()(Session& curSes, DOCID doc)
{
	double coord_x, coord_y, coord_z;
	
	cout << "Point:";
	cout << endl << "Coord x: "; cin >> coord_x;
	cout << endl << "Coord y: "; cin >> coord_y;
	cout << endl << "Coord z: "; cin >> coord_z;

	ConnectData source;
	vector<PointStruct> points;

	points.push_back( PointStruct(coord_x, coord_y, coord_z) );
	source.setPoints(points);

	messageToKern(curSes, doc, source, POINT_FEATURE);
}

void createLineOperation::operator()(Session& curSes, DOCID doc)
{
	double coord_x1, coord_y1, coord_z1;
	double coord_x2, coord_y2, coord_z2;

	cout << "First point:";
	cout << endl << "Coord x: "; cin >> coord_x1;
	cout << endl << "Coord y: "; cin >> coord_y1;
	cout << endl << "Coord z: "; cin >> coord_z1;

	cout << "Second point:";
	cout << endl << "Coord x: "; cin >> coord_x2;
	cout << endl << "Coord y: "; cin >> coord_y2;
	cout << endl << "Coord z: "; cin >> coord_z2;

	ConnectData source;
	vector<PointStruct> points;

	points.push_back( PointStruct(coord_x1, coord_y1, coord_z1) );
	points.push_back( PointStruct(coord_x2, coord_y2, coord_z2) );
	source.setPoints(points);

	messageToKern(curSes, doc, source, LINE_FEATURE);
}

void createCircleOperation::operator()(Session& curSes, DOCID doc)
{
	double coord_x1, coord_y1, coord_z1;
	double coord_x2, coord_y2, coord_z2;

	cout << "Center point:";
	cout << endl << "Coord x: "; cin >> coord_x1;
	cout << endl << "Coord y: "; cin >> coord_y1;
	cout << endl << "Coord z: "; cin >> coord_z1;

	cout << "Side point:";
	cout << endl << "Coord x: "; cin >> coord_x2;
	cout << endl << "Coord y: "; cin >> coord_y2;
	cout << endl << "Coord z: "; cin >> coord_z2;

	ConnectData source;
	vector<PointStruct> points;

	points.push_back( PointStruct(coord_x1, coord_y1, coord_z1) );
	points.push_back( PointStruct(coord_x2, coord_y2, coord_z2) );
	source.setPoints(points);

	messageToKern(curSes, doc, source, CIRCLE_FEATURE);
}

void createArcOperation::operator()(Session& curSes, DOCID doc)
{
	double coord_x1, coord_y1, coord_z1;
	double coord_x2, coord_y2, coord_z2;
	double angle;

	cout << "Center point:";
	cout << endl << "Coord x: "; cin >> coord_x1;
	cout << endl << "Coord y: "; cin >> coord_y1;
	cout << endl << "Coord z: "; cin >> coord_z1;

	cout << "Start point:";
	cout << endl << "Coord x: "; cin >> coord_x2;
	cout << endl << "Coord y: "; cin >> coord_y2;
	cout << endl << "Coord z: "; cin >> coord_z2;

	cout << "Angle: "; cin >> angle;

	ConnectData source;
	vector<PointStruct> points;
	vector<double> dnumbers;

	points.push_back( PointStruct(coord_x1, coord_y1, coord_z1) );
	points.push_back( PointStruct(coord_x2, coord_y2, coord_z2) );
	source.setPoints(points);
		
	dnumbers.push_back(angle);
	source.setDnums(dnumbers);

	messageToKern(curSes, doc, source, ARC_FEATURE);
}

void createPolyLineOperation::operator()(Session& curSes, DOCID doc)
{
	size_t size;
	cout << "Enter the number of lines:"; cin >> size;

	ConnectData source;
	vector<PointStruct> points;
	double coord_x, coord_y, coord_z;

	cout << "First point:";
	cout << endl << "Coord x: "; cin >> coord_x;
	cout << endl << "Coord y: "; cin >> coord_y;
	cout << endl << "Coord z: "; cin >> coord_z;

	points.push_back( PointStruct(coord_x, coord_y, coord_z) );

	for(unsigned i = 0; i < size; i++)
	{
		double coord_x, coord_y, coord_z;

		cout << "Next point:";
		cout << endl << "Coord x: "; cin >> coord_x;
		cout << endl << "Coord y: "; cin >> coord_y;
		cout << endl << "Coord z: "; cin >> coord_z;

		points.push_back( PointStruct(coord_x, coord_y, coord_z) );
	}
		
	source.setPoints(points);
		
	messageToKern(curSes, doc, source, POLYLINE_FEATURE);
}

void setLinePointOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID lineID;
	double coord_x, coord_y, coord_z;

	cout << "Line ID (object of extend):"; cin >> lineID;
		
	cout << "New point:";
	cout << endl << "Coord x: "; cin >> coord_x;
	cout << endl << "Coord y: "; cin >> coord_y;
	cout << endl << "Coord z: "; cin >> coord_z;

	vector<OBJID> ids;
	ids.push_back(lineID);

	vector<PointStruct> points;
	points.push_back( PointStruct(coord_x, coord_y, coord_z) );

	ConnectData source;
	source.setIDs(ids);
	source.setPoints(points);
		
	messageToKern(curSes, doc, source, SET_LINE_POINT_FEATURE);
}

void createMeshOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID geometryID;
	int numberFE;
	cout << "Enter ID (geometry):"; cin >> geometryID;
	cout << "Enter number of EF:"; cin >> numberFE;
		
	vector<OBJID> ids;
	vector<int> inumbers;

	ids.push_back(geometryID);
	inumbers.push_back(numberFE);

	ConnectData source;
	source.setIDs(ids);
	source.setInums(inumbers);
		
	messageToKern(curSes, doc, source, MESH_FEATURE);
}

void createSupportOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID meshID;
	int dof;
	cout << "Enter ID (mesh):"; cin >> meshID;
	cout << "Enter DOF:"; cin >> dof;
		
	vector<OBJID> ids;
	vector<int> inumbers;

	ids.push_back(meshID);
	inumbers.push_back(dof);

	ConnectData source;
	source.setIDs(ids);
	source.setInums(inumbers);
		
	messageToKern(curSes, doc, source, SUPPORT_FEATURE);
}

void createLoadOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID meshID;
	int dof;
	double value;

	cout << "Enter ID (mesh):"; cin >> meshID;
	cout << "Enter DOF:"; cin >> dof;
	cout << "Enter value:"; cin >> value;
		
	vector<OBJID> ids;
	vector<int> inumbers;
	vector<double> dnumbers;

	ids.push_back(meshID);
	inumbers.push_back(dof);
	dnumbers.push_back(value);

	ConnectData source;
	source.setIDs(ids);
	source.setInums(inumbers);
	source.setDnums(dnumbers);
		
	messageToKern(curSes, doc, source, LOAD_FEATURE);
}

void measureDistanceOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID firstPointID, secondPointID;
	cout << "Enter first point ID: "; cin >> firstPointID;
	cout << "Enter second point ID: "; cin >> secondPointID;

	double distance = 0;

	PointStruct begin = curSes.getDocument(doc)->getPointCoords(firstPointID);
	PointStruct end = curSes.getDocument(doc)->getPointCoords(secondPointID);
		
	distance = sqrt( pow((begin.x - end.x), 2) + 
						pow((begin.y - end.y), 2) + 
						pow((begin.z - end.z), 2) );
				
	cout << "Distance: " << distance << endl;
}

void changeThicknessOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID geometryID;
	cout << "Enter geometry ID: "; cin >> geometryID;

	THICKNESS thickness;
	int number = 0;
	cout << "Enter new thickness: "; cin >> number;
	thickness = static_cast<THICKNESS>(number);

	//обращение к ядру: изменить обобщенный объект (generic) по id
	curSes.getDocument(doc)->changeThickness(geometryID, thickness);
}

void changeColorOperation::operator()(Session& curSes, DOCID doc)
{
	OBJID geometryID;
	cout << "Enter geometry ID: "; cin >> geometryID;

	COLOR color;
	int number = 0;
	cout << "Enter new color: "; cin >> number;
	color = static_cast<COLOR>(number);

	//обращение к ядру: изменить обобщенный объект (generic) по id
	curSes.getDocument(doc)->changeColor(geometryID, color);
}

void changeCacheUndoRedo::operator()(Session& curSes, DOCID doc)
{
	unsigned cache = 0;
	cout << "Enter new cache: "; cin >> cache;
				
	//обращение к ядру
	curSes.getDocument(doc)->changeCacheUndoRedo(cache);
}

void selectLayers::operator()(Session& curSes, DOCID doc)
{
	vector<unsigned> newLayers;
	istream_iterator<unsigned> start(cin), end;
		
	cout << "Select layers to show and press <Ctrl+Z>: ";
		
	while(start != end)
		newLayers.push_back(*start++);
		
	curSes.getDocument(doc)->selectLayers(newLayers);
}