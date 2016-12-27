#include <iostream>
#include <vector>
#include <algorithm>
#include "test.h"

int main()
{
	test ob1, ob2(3);
	std::vector<test> data;

	data.push_back(ob1);
	data.push_back(ob2);

	std::for_each(data.begin(), data.end(),
		[](const test& object) { object.show(); });
	
	system("pause");
	return 0;
}