#include "document.h"

Document::Document(void* hwnd)
{
	_base.attachObserver(&_buffer);
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

	_hrc = wglCreateContext(_dc);

	wglMakeCurrent(_dc, _hrc);

	int w = 200, h = 200;
	int aspectratio = w / h;
	glViewport(0, 0, w, h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(45.0f, aspectratio, 0.0001f, 500.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}

Document::~Document()
{
	wglMakeCurrent(nullptr, nullptr);
	wglDeleteContext(_hrc);
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

void Document::toScreen(void)
{
	glClear(GL_COLOR_BUFFER_BIT);
	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

	glLoadIdentity();
	gluLookAt(1.0, 1.0, 5.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);

	glPointSize(5);
	_buffer.toScreen();

	SwapBuffers(_dc);
}
