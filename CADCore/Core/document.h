/*This file contains the document class*/

#pragma once
#include "definitions.h"
#include "base.h"

class Settings	//document settings
{
private:
	COLOR _backgroundColor;				//color of background
	COLOR _defaultEdgeColor;			//default edge color
	THICKNESS _defaultEdgeThickness;	//default edge thickness
	bool _showID;						//show or not edges ids

public:
	Settings(): _backgroundColor(BLUE), _defaultEdgeColor(BLACK),
		_defaultEdgeThickness(THREE), _showID(false) {}

	bool getShowID(void) { return _showID; }
	COLOR getBackgroundColor(void) { return _backgroundColor; }
	COLOR getDefaultEdgeColor(void) { return _defaultEdgeColor; }
	THICKNESS getDefaultEdgeThickness(void) { return _defaultEdgeThickness; }

	void setShowID(bool mode) { _showID = mode; }
	void setBackgroundColor(COLOR color) { _backgroundColor = color; }
	void setDefaultEdgeColor(COLOR color) { _defaultEdgeColor = color; }
	void setDefaultEdgeThickness(THICKNESS thickness) { _defaultEdgeThickness = thickness; }
};

class Buffer	//contains all objects which are drawn on the screen
{
private:
	std::vector<Generic*> _buffer;	//the objects that appear on the screen
	std::vector<unsigned> _layers;	//the layers that appear on the screen

public:
	Buffer() { _layers.push_back(0); }

	void setLayers(std::vector<unsigned>& newLayers) { _layers = newLayers; }
	
	void attachToBuffer(Generic* object);		//to display generic on the screen
	void detachFrombuffer(Generic* object);		//to remove generic from the screen

	void toScreen(void);
};

class Document
{
private:
	Base _base;				//user data
	Buffer _buffer;			//the objects that appear on the screen
	Settings _settings;		//document settings

public:
	//todo
};