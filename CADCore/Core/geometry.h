/*This file contains all the math of this project*/

#pragma once

class Node	//additional node
{
private:
	double _coord_X;
	double _coord_Y;
public:
	double getX(void) { return _coord_X; }
	double getY(void) { return _coord_Y; }
	void setX(double x) { _coord_X = x; }
	void setY(double y) { _coord_Y = y; }
};

class Math {};	//todo
class LineLaw: public Math {};	//todo
class CircleLaw: public Math {};	//todo