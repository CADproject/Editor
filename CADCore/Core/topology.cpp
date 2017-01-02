#include "topology.h"

std::vector<Node*> Point::drawing(void)
{
	std::vector<Node*> vect;
	vect.push_back(&_node);
	return vect;
}

std::vector<Node*> Line::drawing(void)
{
	//todo
}

std::vector<Node*> Circle::drawing(void)
{
	//todo
}