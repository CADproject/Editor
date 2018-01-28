#include "document.h"

static bool staticInitComplete = false;
static std::map<COLOR, float*> colors;

Settings::Settings() : _backgroundColor(BLACK), _defaultEdgeColor(WHITE),
_defaultEdgeThickness(THREE), _showID(false)
{
	if (!staticInitComplete)
	{
		staticInitComplete = true;
		colors[BLACK]	= new float[4]{ 0.212f, 0.212f, 0.212f, 0 };	//grey21
		colors[RED]		= new float[4]{ 1,      0.549f, 0.412f, 1 };	//Salmon1
		colors[GREEN]	= new float[4]{ 0.306f, 0.933f, 0.580f, 1 };	//SeaGreen2	
		colors[BLUE]	= new float[4]{ 0.392f, 0.583f, 0.929f, 1 };	//CornflowerBlue
		colors[YELLOW]	= new float[4]{ 0.804f, 0.804f, 0,      1 };	//Yellow3
		colors[WHITE]	= new float[4]{ 0.961f, 0.961f, 0.961f, 1 };	//WhiteSmoke
	}
}

Document::Document(void* hwnd)
{
	_base.attachObserver(&_buffer);
	_active = false;

	_height = 480;
}

Document::Document()
{

}

Document::~Document()
{
}

ObjectId Document::attachToBase(Generic* object)
{
	return _base.attachToBase(object);
	callBackFunction draw = MapGetValue<std::string, callBackFunction>(_callbacks, callbacks::DrawGeometry);
	if (draw != nullptr)
		;
}

Generic* Document::detachFromBase(ObjectId objID)
{
	return _base.detachFromBase(objID);
}

Generic* Document::getGeneric(ObjectId objID)
{
	return _base.getGeneric(objID);
}

void Document::attachToBuffer(Generic* object)
{
	_buffer.attachToBuffer(object);
}

void Document::detachFrombuffer(Generic* object)
{
	_buffer.detachFrombuffer(object);
}

Topology* Document::getGenericTopology(ObjectId objID)
{
	return _base.getGeneric(objID)->getTopology();
}

void Document::commit(void)
{
	_base.commit();
}

void Document::undo(void)
{
	_base.undo();
}

void Document::redo(void)
{
	_base.redo();
}

void Document::setLayers(std::vector<unsigned>& newLayers)
{
	_buffer.setLayers(newLayers);
}

void Document::setBackgroundColor(COLOR color)
{
	_settings.setBackgroundColor(color);
}

std::vector<unsigned> Document::getLayers(void)
{
	return _buffer.getLayers();
}

COLOR Document::getBackgroundColor(void)
{
	return _settings.getBackgroundColor();
}

void Document::InitCallBacks(std::map<std::string, callBackFunction> callbacks)
{
	_callbacks = callbacks;
}
