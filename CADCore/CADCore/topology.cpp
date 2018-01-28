#include "topology.h"

void Point::drawing(void) const
{
	//glBegin(GL_POINTS);
	//glVertex2f(_node.getX(), _node.getY());
	//glEnd();

#ifdef TRACE_DEBUG
	std::cout << _law.getType();
	std::cout << ". Node: ";
	std::cout << "X = " << _node.getX();
	std::cout << ", Y = " << _node.getY() << ".";
	std::cout << std::endl;
#endif
}

void Line::drawing(void) const
{
	//glBegin(GL_LINES);
	//glVertex2f(_start.getX(), _start.getY());
	//glVertex2f(_end.getX(), _end.getY());
	//glEnd();

#ifdef TRACE_DEBUG
	std::cout << _law.getType();
	std::cout << ". Start: ";
	std::cout << "X = " << _start.getX();
	std::cout << ", Y = " << _start.getY();
	std::cout << ", End: ";
	std::cout << "X = " << _end.getX();
	std::cout << ", Y = " << _end.getY() << ".";
	std::cout << std::endl;
#endif
}

void DrawCircle(float cx, float cy, float r, int num_segments)
{
	//glBegin(GL_LINE_LOOP);
	//for (int ii = 0; ii < num_segments; ii++)
	//{
	//	float theta = 2.0f * 3.1415926f * float(ii) / float(num_segments);//get the current angle

	//	float x = r * cosf(theta);//calculate the x component
	//	float y = r * sinf(theta);//calculate the y component

	//	glVertex2f(x + cx, y + cy);//output vertex

	//}
	//glEnd();
}

void Circle::drawing(void) const
{
	//float x = _center.getX() - _side.getX(), y = _center.getY() - _side.getY();
	//float r = sqrt(x*x + y*y);
	//DrawCircle(_center.getX(), _center.getY(), r, 48);

#ifdef TRACE_DEBUG
	std::cout << _law.getType();
	std::cout << ". Center: ";
	std::cout << "X = " << _center.getX();
	std::cout << ", Y = " << _center.getY();
	std::cout << ", Side: ";
	std::cout << "X = " << _side.getX();
	std::cout << ", Y = " << _side.getY() << ".";
	std::cout << std::endl;
#endif
}

void Contour::drawing(void) const
{
#ifdef TRACE_DEBUG
	std::cout << "CONTOUR:" << std::endl;
#endif

	std::for_each(_edges.begin(), _edges.end(),
		[](Edge* curEdge)
	{
#ifdef TRACE_DEBUG
		std::cout << "\t";
#endif
		curEdge->drawing();
	});
}