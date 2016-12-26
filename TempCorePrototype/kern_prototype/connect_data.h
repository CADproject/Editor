#pragma once
#include "headers.h"
using namespace std;

//связующая структура данных для узлов дерева модели

class ConnectData
{
private:
	vector<PointStruct> _points;
	vector<int> _inums;
	vector<double> _dnums;
	vector<OBJID> _ids;
public:
	ConnectData() {}
	~ConnectData() {}

	ConnectData& operator=(const ConnectData& op2)
	{
		_points = op2._points;
		_inums = op2._inums;
		_dnums = op2._dnums;
		_ids = op2._ids;
		return *this;
	}

	vector<PointStruct> getPoints(void) { return _points; }
	vector<int> getInums(void) { return _inums; }
	vector<double> getDnums(void) { return _dnums; }
	vector<OBJID> getIDs(void) { return _ids; }

	void setPoints(const vector<PointStruct>& points) { _points = points; }
	void setInums(const vector<int>& inums) { _inums = inums; }
	void setDnums(const vector<double>& dnums) { _dnums = dnums; }
	void setIDs(const vector<OBJID>& ids) { _ids = ids; }
};