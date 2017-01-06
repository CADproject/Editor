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

class Document
{
private:
	Buffer _buffer;			//the objects that appear on the screen
	Base _base;				//user data
	Settings _settings;		//document settings

public:
	Document() { _base.attachObserver(&_buffer); }

	OBJID attachToBase(Generic* object);
	void detachFromBase(OBJID objID);
	Generic* getGeneric(OBJID objID);

	void undo(void);
	void redo(void);
};