#include <algorithm>
#include "topology.h"

std::vector<Node> Point::drawing(void) const
{
	std::vector<Node> nodes;
	nodes.push_back(_node);
	return nodes;
}

std::vector<Node> Line::drawing(void) const
{
	//todo
	return std::vector<Node>();
}

std::vector<Node> Circle::drawing(void) const
{
	//todo
	return std::vector<Node>();
}

std::vector< std::vector<Node> > Contour::drawing(void) const
{
	std::vector< std::vector<Node> > container;

	std::for_each(_edges.begin(), _edges.end(),
		[&container](const Edge* curEdge) { container.push_back( curEdge->drawing() ); });
	
	return container;
}