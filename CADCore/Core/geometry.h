/*This file contains all the math of this project*/

#pragma once
#include "headers.h"

class Node	//additional node
{
private:
	double _coord_X;
	double _coord_Y;
public:
	Node(): _coord_X(0), _coord_Y(0) {}
	
	Node(double x, double y)
	{
		_coord_X = x;
		_coord_Y = y;
	}

	double getX(void) const { return _coord_X; }
	double getY(void) const { return _coord_Y; }
	void setX(double x) { _coord_X = x; }
	void setY(double y) { _coord_Y = y; }
};

class Math	//use Singleton in Future?
{
protected:
	std::string _type;
public:
	std::string getType(void) const { return _type; }
};

class PointLaw: public Math
{
public:
	PointLaw() { _type = "POINT"; }
};

class LineLaw: public Math
{
public:
	LineLaw() { _type = "LINE";}
};

class CircleLaw: public Math
{
public:
	CircleLaw() { _type = "CIRCLE"; }
};