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
	callBackFunction draw = MapGetValue<std::string, callBackFunction>(_callbacks, callbacks::DrawGeometry);
	if (draw != nullptr)
	{
		auto nodes = object->getTopology()->GetDrawPoints();
		CallbackValues values;
		values.id = object->GetId();
		values.flag = callbacks::GeometryAction::Update;
		values.line = "pinvoked!";
		values.thickness = 1;
		values.FillbyNodes(nodes);
		values.pInt = new int[3]{ 255, 20, 255 };
		values.pString = new char[values.size * 100];
		memset(values.pString, 0, values.size * 100);
		draw(values);
	}
	return _base.attachToBase(object);
}

Generic* Document::detachFromBase(ObjectId objID)
{
	callBackFunction draw = MapGetValue<std::string, callBackFunction>(_callbacks, callbacks::DrawGeometry);
	if (draw != nullptr)
	{
		auto object = _base.getGeneric(objID);
		auto nodes = object->getTopology()->GetDrawPoints();
		CallbackValues values;
		values.id = object->GetId();
		values.flag = callbacks::GeometryAction::Remove;
		draw(values);
	}

	return _base.detachFromBase(objID);
}

Generic* Document::getGeneric(ObjectId objID)
{
	return _base.getGeneric(objID);
}

void Document::attachToBuffer(Generic* object)
{
	_buffer.attachToBuffer(object);

	callBackFunction draw = MapGetValue<std::string, callBackFunction>(_callbacks, callbacks::DrawGeometry);
	if (draw != nullptr)
	{
		auto nodes = object->getTopology()->GetDrawPoints();
		CallbackValues values;
		values.id = object->GetId();
		values.flag = callbacks::GeometryAction::Update;
		values.line = "pinvoked!";
		values.thickness = 1;
		values.FillbyNodes(nodes);
		values.pInt = new int[3]{ 255, 20, 255 };
		values.pString = new char[values.size * 100];
		memset(values.pString, 0, values.size * 100);
		draw(values);
	}
}

void Document::detachFrombuffer(Generic* object)
{
	_buffer.detachFrombuffer(object);

	callBackFunction draw = MapGetValue<std::string, callBackFunction>(_callbacks, callbacks::DrawGeometry);
	if (draw != nullptr)
	{
		auto nodes = object->getTopology()->GetDrawPoints();
		CallbackValues values;
		values.id = object->GetId();
		values.flag = callbacks::GeometryAction::Remove;
		draw(values);
	}
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
