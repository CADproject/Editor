#pragma once
#include "headers.h"
using namespace std;

//иерархия геометрических примитивов

class Geometry
{
public:
	Geometry() {}
	virtual ~Geometry() {}
	virtual void show(void)/* = 0*/ {};
	virtual unsigned getID(void)/* = 0*/ { return 0; };
};

class Point: public Geometry
{
private:
	static POINTID _id;
	PointStruct _point;
public:
	Point() { _id++; }
	Point(const PointStruct& point) { _point = point; _id++; }
	~Point() {}

	void show(void)
	{
		cout << "Geometry point, id: " << _id << endl;
	}

	POINTID getID(void) { return _id; }
	PointStruct getPointCoords(void) { return _point; }
};

class Line: public Geometry
{
private:
	static LINEID _id;
	PointStruct _start;
	PointStruct _end;
public:
	Line() { _id++; }

	Line(const PointStruct& start, const PointStruct& end) 
	{ 
		_start = start;
		_end = end;
		_id++; 
	}

	~Line() {}

	void show(void)
	{
		cout << "Geometry line, id: " << _id << endl;
	}

	LINEID getID(void) { return _id; }
	PointStruct getStart(void) { return _start; }
	PointStruct getEnd(void) { return _end; }
};

class Circle: public Geometry
{
private:
	static CIRCLEID _id;
	PointStruct _center;
	PointStruct _side;
public:
	Circle() { _id++; }

	Circle(const PointStruct& center, const PointStruct& side) 
	{ 
		_center = center;
		_side = side;
		_id++; 
	}

	~Circle() {}

	void show(void)
	{
		cout << "Geometry circle, id: " << _id << endl;
	}

	CIRCLEID getID(void) { return _id; }
};

class Arc: public Geometry
{
private:
	static ARCID _id;
	PointStruct _center;
	PointStruct _start;
	double _angle;
public:
	Arc() { _id++; }

	Arc(const PointStruct& center, const PointStruct& start, double angle) 
	{ 
		_center = center;
		_start = start;
		_angle = angle;
		_id++; 
	}

	~Arc() {}

	void show(void)
	{
		cout << "Geometry arc, id: " << _id << endl;
	}

	ARCID getID(void) { return _id; }
};

class PolyLine: public Geometry
{
private:
	static POLYLINEID _id;
	vector<PointStruct> _points;
public:
	PolyLine() { _id++; }

	PolyLine(const vector<PointStruct>& points) 
	{ 
		_points = points;
		_id++; 
	}

	~PolyLine() {}

	void show(void)
	{
		cout << "Geometry polyline, id: " << _id << endl;
	}

	POLYLINEID getID(void) { return _id; }
};

//примкнувшие к геометрическим примитивам
//объекты физической модели

class Mesh: public Geometry
{
private:
	static MESHID _id;
	Geometry* _geometry;
	int _numberFE;
public:
	Mesh() { _geometry = nullptr; _numberFE = 0; _id++; }

	Mesh(Geometry* geometry, int numberFE) 
	{ 
		_geometry = geometry;
		_numberFE = numberFE;
		_id++;
	}

	~Mesh() {}

	void show(void)
	{
		cout << "Mesh, id: " << _id << ", ";
		cout << "On geometry, id: " << _geometry->getID() << endl;
	}

	Geometry* getGeometry(void) { return _geometry; }
	MESHID getID(void) { return _id; }
};

class Load: public Geometry
{
private:
	static LOADID _id;
	Mesh* _mesh;
	DOF _dof;
	double _value;
public:
	Load() { _mesh = nullptr; _dof = def; _value = 0; _id++; }

	Load(Mesh* mesh, DOF dof, double value) 
	{ 
		_mesh = mesh;
		_dof = dof;
		_value = value;
		_id++;
	}

	~Load() {}

	void show(void)
	{
		cout << "Load, id: " << _id << ", ";
		cout << "On geometry, id: " << _mesh->getGeometry()->getID() << endl;
	}

	LOADID getID(void) { return _id; }
};

class Support: public Geometry
{
private:
	static SUPPORTID _id;
	Mesh* _mesh;
	DOF _dof;
public:
	Support() { _mesh = nullptr; _dof = def; _id++; }

	Support(Mesh* mesh, DOF dof) 
	{ 
		_mesh = mesh;
		_dof = dof;
		_id++;
	}

	~Support() {}

	void show(void)
	{
		cout << "Support, id: " << _id << ", ";
		cout << "On geometry, id: " << _mesh->getGeometry()->getID() << endl;
	}

	SUPPORTID getID(void) { return _id; }
};