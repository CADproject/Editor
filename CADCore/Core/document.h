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

class Document	//data are written to a file, document must be created using operator "new"
{
private:
	Buffer _buffer;			//the objects that appear on the screen
	Base _base;				//user data
	Settings _settings;		//document settings

public:
	Document() { _base.attachObserver(&_buffer); }

	OBJID attachToBase(Generic* object);
	Generic* detachFromBase(OBJID objID);
	Generic* getGeneric(OBJID objID);

	void attachToBuffer(Generic* object);
	void detachFrombuffer(Generic* object);

	Topology* getGenericTopology(OBJID objID);

	void commit(void);
	void undo(void);
	void redo(void);

	void setLayers(std::vector<unsigned>& newLayers);
	void setBackgroundColor(COLOR color);
	void toScreen(void);
};