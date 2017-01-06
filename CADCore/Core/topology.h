/*This file contains a hierarchy of geometric primitives*/

#pragma once
#include <list>
#include <vector>
#include "geometry.h"

class Topology	//base class of all primitives
{
public:
	virtual void drawing(void) const = 0;
};

class Edge: public Topology		//base class of all types of edges
{
protected:
	Math* _law;
};

class Point: public Edge
{
private:
	Node _node;

public:
	Point()
	{
		_node.setX(0.0);
		_node.setY(0.0);
		_law = nullptr;
	}

	Point(const Node& node): _node(node) { _law = new PointLaw(); }

	~Point() { if(_law) delete _law; }
	
	/*virtual*/ void drawing(void) const;
};

class Line: public Edge
{
private:
	Node _start;
	Node _end;

public:
	Line()
	{
		_start.setX(0.0);
		_start.setY(0.0);
		_end.setX(0.0);
		_end.setY(0.0);
		_law = nullptr;
	}

	Line(const Node& start, const Node& end): _start(start), _end(end)
	{
		_law = new LineLaw();
	}

	~Line() { if(_law) delete _law; }

	/*virtual*/ void drawing(void) const;
};

class Circle: public Edge
{
private:
	Node _center;
	Node _side;

public:
	Circle()
	{
		_center.setX(0.0);
		_center.setY(0.0);
		_side.setX(0.0);
		_side.setY(0.0);
		_law = nullptr;
	}

	Circle(const Node& center, const Node& side): _center(center), _side(side)
	{
		_law = new CircleLaw();
	}

	~Circle() { if(_law) delete _law; }

	/*virtual*/ void drawing(void) const;
};

class Contour: public Topology	//contour is a container of edges
{
private:
	std::list<Edge*> _edges;

public:
	Contour() {}
	Contour(const std::vector<Edge*>& edges) { _edges.assign(edges.begin(), edges.end()); }

	/*virtual*/ void drawing(void) const;
};