/*This file contains a hierarchy of geometric primitives*/

#pragma once
#include "headers.h"

class Topology	//base class of all primitives
{
public:
	virtual void drawing(void) const = 0;
	virtual std::vector<Node> GetDrawPoints(void) = 0;
};

class Edge: public Topology		//base class of all types of edges
{
protected:
	Math _law;		//Math* _law;
};

class Point: public Edge
{
private:
	Node _node;

public:
	Point()
	{
		_node.X = (0.0);
		_node.Y = (0.0);
		_law = PointLaw();
	}

	Point(const Node& node): _node(node) { _law = PointLaw(); }
	//~Point() { if(_law) delete _law; }
	
	/*virtual*/ void drawing(void) const;
	virtual std::vector<Node> GetDrawPoints(void);
};

class Line: public Edge
{
private:
	Node _start;
	Node _end;

public:
	Line()
	{
		_start.X = (0.0);
		_start.Y = (0.0);
		_end.X = (0.0);
		_end.Y = (0.0);
		_law = LineLaw();
	}

	Line(const Node& start, const Node& end): _start(start), _end(end)
	{
		_law = LineLaw();
	}

	//~Line() { if(_law) delete _law; }

	/*virtual*/ void drawing(void) const;
	virtual std::vector<Node> GetDrawPoints(void);
};

class Circle: public Edge
{
private:
	Node _center;
	Node _side;

public:
	Circle()
	{
		_center.X = (0.0);
		_center.Y = (0.0);
		_side.X = (0.0);
		_side.Y = (0.0);
		_law = CircleLaw();
	}

	Circle(const Node& center, const Node& side): _center(center), _side(side)
	{
		_law = CircleLaw();
	}

	//~Circle() { if(_law) delete _law; }

	/*virtual*/ void drawing(void) const;
	virtual std::vector<Node> GetDrawPoints(void);
};

class Contour: public Topology	//contour is a container of edges
{
private:
	std::vector<Edge*> _edges;

public:
	Contour() {}
	Contour(const std::vector<Edge*>& edges) { _edges = edges; }

	std::vector<Edge*> getEdges(void) { return _edges; }
	/*virtual*/ void drawing(void) const;
	virtual std::vector<Node> GetDrawPoints(void);
};