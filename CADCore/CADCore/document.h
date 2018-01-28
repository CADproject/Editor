/*This file contains the document class*/

#pragma once
#include "headers.h"
#include "base.h"

class Settings	//document settings
{
private:
	COLOR _backgroundColor;				//color of background
	COLOR _defaultEdgeColor;			//default edge color
	THICKNESS _defaultEdgeThickness;	//default edge thickness
	bool _showID;						//show or not edges ids

public:
	Settings();

	bool getShowID(void) { return _showID; }
	COLOR getBackgroundColor(void) { return _backgroundColor; }
	COLOR getDefaultEdgeColor(void) { return _defaultEdgeColor; }
	THICKNESS getDefaultEdgeThickness(void) { return _defaultEdgeThickness; }

	void setShowID(bool mode) { _showID = mode; }
	void setBackgroundColor(COLOR color) { _backgroundColor = color; }
	void setDefaultEdgeColor(COLOR color) { _defaultEdgeColor = color; }
	void setDefaultEdgeThickness(THICKNESS thickness) { _defaultEdgeThickness = thickness; }
};

class Document	//data are written to a file, document must be created using operator "new"
{
private:
	Buffer _buffer;			//the objects that appear on the screen
	Base _base;				//user data
	Settings _settings;		//document settings
	volatile bool _active;
	Document();
	std::map<std::string, callBackFunction> _callbacks;
public:
	double _height;

	Document(void* hwnd);
	~Document();

	void InitCallBacks(std::map<std::string, callBackFunction> callbacks);

	ObjectId attachToBase(Generic* object);
	Generic* detachFromBase(ObjectId objID);
	Generic* getGeneric(ObjectId objID);

	void attachToBuffer(Generic* object);
	void detachFrombuffer(Generic* object);

	Topology* getGenericTopology(ObjectId objID);

	void commit(void);
	void undo(void);
	void redo(void);

	void setLayers(std::vector<unsigned>& newLayers);
	void setBackgroundColor(COLOR color);

	std::vector<unsigned> getLayers(void);
	COLOR getBackgroundColor(void);
};
