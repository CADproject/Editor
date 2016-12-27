class test
{
private:
	int _x;
public:
	test(): _x(0) {}
	test(int value): _x(value) {}

	void show(void) const { std::cout << "X: " << _x << std::endl; }
};