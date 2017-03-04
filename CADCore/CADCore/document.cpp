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
	if (hwnd == nullptr) return;

	_hwnd = (HWND)hwnd;

	PIXELFORMATDESCRIPTOR pfd =
	{
		sizeof(PIXELFORMATDESCRIPTOR),
		1,
		PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER,    //Flags
		PFD_TYPE_RGBA,            //The kind of framebuffer. RGBA or palette.
		32,                        //Colordepth of the framebuffer.
		0, 0, 0, 0, 0, 0,
		0,
		0,
		0,
		0, 0, 0, 0,
		24,                        //Number of bits for the depthbuffer
		8,                        //Number of bits for the stencilbuffer
		0,                        //Number of Aux buffers in the framebuffer.
		PFD_MAIN_PLANE,
		0,
		0, 0, 0
	};

	_dc = GetDC((HWND)_hwnd);

	int pfc = ChoosePixelFormat(_dc, &pfd);
	SetPixelFormat(_dc, pfc, &pfd);

	_height = 480;
}

Document::~Document()
{
	RenderDeactivateContext();
	ReleaseDC(_hwnd, _dc);
}

ObjectId Document::attachToBase(Generic* object)
{
	return _base.attachToBase(object);
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

void Document::RenderDraw()
{
	if (!_active) return;

	glClear(GL_COLOR_BUFFER_BIT);

	auto color = colors[getBackgroundColor()];
	glClearColor(color[0], color[1], color[2], color[3]);

	glLoadIdentity();

	gluLookAt(0.0, 0.0, pow(2.71828182845904523536, _height/360.0), 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);


	glColor3f(1, 1, 1);

	glPointSize(5);
	_buffer.toScreen();

	SwapBuffers(_dc);
}

void Document::RenderResize(int w, int h)
{
	float aspectratio = (float)w / (float)h;
	glViewport(0, 0, w, h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	const double pi180 = 3.14 / 180.0;
	auto fov1 = atan(tan(pi180 * 45 / 2.0) * h / 400.0) * 2.0 / pi180;

	gluPerspective(fov1, aspectratio, 0.0001f, 500.0f);
	glMatrixMode(GL_MODELVIEW);
	
	glLoadIdentity();
}

void Document::RenderDeactivateContext()
{
	_active = false;
	wglMakeCurrent(nullptr, nullptr);
	wglDeleteContext(_hrc);
}

void Document::RenderActivateContext(int w, int h)
{
	_hrc = wglCreateContext(_dc);
	wglMakeCurrent(_dc, _hrc);
	RenderResize(w, h);
	_active = true;
}
