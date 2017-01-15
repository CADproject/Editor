#include "session.h"

//session factory method
Session* sessionFactory(void);

//document factory method
Document* documentFactory(void);

//node factory method
Node* nodeFactory(double x, double y);

//point factory method
Point* pointFactory(const Node& node);

//line factory method
Line* lineFactory(const Node& start, const Node& end);

//circle factory method
Circle* circleFactory(const Node& center, const Node& side);

//contour factory method
Contour* contourFactory(const std::vector<Edge*>& edges);

//generic factory methods
Generic* genericFactory(Topology* primitive);
Generic* genericFactory(unsigned layer, Topology* primitive);
Generic* genericFactory(unsigned layer, COLOR color, THICKNESS thickness, Topology* primitive);