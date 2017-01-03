/*This file contains the document class*/

#pragma once
#include "definitions.h"
#include "undo_redo.h"
#include "base.h"

class Settings	//document settings
{
private:
	bool _showID;						//show or not edges ids
	COLOR _backgroundColor;				//color of background
	COLOR _defaultEdgeColor;			//default edge color
	THICKNESS _defaultEdgeThickness;	//default edge thickness

public:
	bool getShowID(void) { return _showID; }
	COLOR getBackgroundColor(void) { return _backgroundColor; }
	COLOR getDefaultEdgeColor(void) { return _defaultEdgeColor; }
	THICKNESS getDefaultEdgeThickness(void) { return _defaultEdgeThickness; }

	void setShowID(bool mode) { _showID = mode; }
	void setBackgroundColor(COLOR color) { _backgroundColor = color; }
	void setDefaultEdgeColor(COLOR color) { _defaultEdgeColor = color; }
	void getDefaultEdgeThickness(THICKNESS thickness) { _defaultEdgeThickness = thickness; }
};

class Document
{
private:
	//static DOCID _id;		//ids counter
	Base _base;				//user data
	Buffer _buffer;			//the objects that appear on the screen
	UndoRedo _undoredo;		//undo/redo mechanism
	Settings _settings;		//document settings

public:
	//todo
};