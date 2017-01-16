#include "core_api.h"

Session* sessionFactory(void)
{
	return new Session();
}

Document* documentFactory(void)
{
	return new Document();
}

Node* nodeFactory(double x, double y)
{
	return new Node(x, y);
}

Point* pointFactory(const Node& node)
{
	return new Point(node);
}

Line* lineFactory(const Node& start, const Node& end)
{
	return new Line(start, end);
}

Circle* circleFactory(const Node& center, const Node& side)
{
	return new Circle(center, side);
}

Contour* contourFactory(const std::vector<Edge*>& edges)
{
	return new Contour(edges);
}

Generic* genericFactory(Topology* primitive, unsigned layer, COLOR color, THICKNESS thickness)
{
	if(layer == 0 && color == BLACK && thickness == THREE)
	{
		return new Generic(primitive);
	}
	else if(color == BLACK && thickness == THREE)
	{
		return new Generic(layer, primitive);
	}
	else
	{
		return new Generic(layer, color, thickness, primitive);
	}
}