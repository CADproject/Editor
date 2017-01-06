#include <iostream>
#include <algorithm>
#include "topology.h"

void Point::drawing(void) const
{
	std::cout << "Type: " << _law->getType();
	std::cout << ", Node: ";
	std::cout << "X: " << _node.getX();
	std::cout << "Y: " << _node.getY();
	std::cout << std::endl;
}

void Line::drawing(void) const
{
	std::cout << "Type: " << _law->getType();
	std::cout << ", Start: ";
	std::cout << "X: " << _start.getX();
	std::cout << "Y: " << _start.getY();
	std::cout << ", End: ";
	std::cout << "X: " << _end.getX();
	std::cout << "Y: " << _end.getY();
	std::cout << std::endl;
}

void Circle::drawing(void) const
{
	std::cout << "Type: " << _law->getType();
	std::cout << ", Center: ";
	std::cout << "X: " << _center.getX();
	std::cout << "Y: " << _center.getY();
	std::cout << ", Side: ";
	std::cout << "X: " << _side.getX();
	std::cout << "Y: " << _side.getY();
	std::cout << std::endl;
}

void Contour::drawing(void) const
{
	std::cout << "Conour:" << std::endl;

	std::for_each(_edges.begin(), _edges.end(),
		[](Edge* curEdge)
	{
		std::cout << "\t";
		curEdge->drawing();
	});
}