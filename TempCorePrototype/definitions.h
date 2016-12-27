#pragma once

typedef unsigned DOCID;
typedef unsigned NODEID;
typedef unsigned OBJID;

typedef unsigned POINTID;
typedef unsigned LINEID;
typedef unsigned CIRCLEID;
typedef unsigned ARCID;
typedef unsigned POLYLINEID;
typedef unsigned MESHID;
typedef unsigned LOADID;
typedef unsigned SUPPORTID;

enum DOF {def, tx, ty, tz, rx, ry, rz};
enum COLOR {BLACK, RED, GREEN, BLUE};
enum THICKNESS {ONE, TWO, THREE, FOUR};

enum FEATID {ABSTRUCT_FEATURE,
				POINT_FEATURE,
				LINE_FEATURE, 
				CIRCLE_FEATURE,
				ARC_FEATURE,
				POLYLINE_FEATURE,
				SET_LINE_POINT_FEATURE,
				MESH_FEATURE,
				SUPPORT_FEATURE,
				LOAD_FEATURE};

//вспомогательная структура для построения геометрических примитивов
struct PointStruct
{
	double x, y, z;

	PointStruct() { x = y = z = 0; }
	PointStruct(double x, double y, double z) 
	{ 
		this->x = x; 
		this->y = y; 
		this->z = z; 
	}
};