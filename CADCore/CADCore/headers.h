#pragma once

#include <iostream>
#include <cassert>

#include <map>
#include <list>
#include <string>
#include <vector>
#include <deque>

#include <utility>
#include <algorithm>

#include "definitions.h"
#include "geometry.h"
#include "topology.h"

#pragma region Callback functions names
namespace callbacks
{
	//==================================================
	// Графика
	//==================================================

	enum GeometryAction
	{
		Update = 0,
		ClearAll = 1,
		Remove = 2
	};

	/// <summary>
	/// отрисовка геометрии
	/// </summary>
	/// <param name="value">int[] color, int thickness, double[] points, int size</param>
	extern const char* DrawGeometry;

	/// <summary>
	/// задание фона документа
	/// </summary>
	/// <param name="value">int color</param>
	extern const char* Background;

	/// <summary>
	/// показать / скрыть узлы
	/// </summary>
	/// <param name="value">int color, int thickness</param>
	extern const char* DrawNodes;

	/// <summary>
	/// показать / скрыть сетку
	/// </summary>
	/// <param name="value">int color, int thickness</param>
	extern const char* DrawMesh;

	/// <summary>
	/// задать положение камеры
	/// </summary>
	/// <param name="value">double[] pos</param>
	extern const char* SetCameraPosition;

	//==================================================
	// Текст
	//==================================================

	/// <summary>
	/// вывести текст в первую строку
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	extern const char* FirstString;

	/// <summary>
	/// вывести текст во вторую строку
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	extern const char* SecondString;

	/// <summary>
	/// вывести текст на консоль
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	extern const char* ConsoleLog;

	//==================================================
	// Слои
	//==================================================

	/// <summary>
	/// отправить список слоев (для отображения в UI)
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	extern const char* LayersList;

	/// <summary>
	/// отправить список видимых слоев (для отображения в UI)
	/// </summary>
	/// <param name="value">int[] ids, int size</param>
	extern const char* VisibleLayers;

	/// <summary>
	/// задать активный слой (для отображения в UI)
	/// </summary>
	/// <param name="value">int LayerId</param>
	extern const char* SetActiveLayer;

	//==================================================
	// Настройки
	//==================================================

	/// <summary>
	/// задать имя документу (для отображения в UI)
	/// </summary>
	/// <param name="value">int docId, string str, int size</param>
	extern const char* SetDocName;

	/// <summary>
	/// отправить список документов
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	extern const char* DocsList;

	/// <summary>
	/// вернуть состояние документа (сохраненный или нет)
	/// </summary>
	/// <param name="value">int id, bool status</param>
	extern const char* SetDocState;

	/// <summary>
	/// вернуть количество всех объектов/по типам/слоев
	/// </summary>
	/// <param name="value">int[] objects, int size</param>
	extern const char* SetDocStatistics;

	/// <summary>
	/// задать активную тему (для отображения в UI)
	/// </summary>
	/// <param name="value">int themeId</param>
	extern const char* SetActiveTheme;

	/// <summary>
	/// отправить список тем (для отображения в UI)
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	extern const char* ThemesList;
}
#pragma endregion Callback functions names

#pragma pack(push, 1)
struct CallbackValues
{
public:
	__int64 id = 0;
	double thickness = 0;
	int size = 0;
	char* line = nullptr;
	int flag = 0;
	char* pString = nullptr;
	int* pInt = nullptr;
	double* pDouble = nullptr;

	void FillbyNodes(std::vector<Node> nodes)
	{
		size = nodes.size();
		pDouble = new double[size * 3];
		for (int i = 0; i < size; i++)
		{
			pDouble[i*3 + 0] = nodes[i].X;
			pDouble[i*3 + 1] = nodes[i].Y;
			pDouble[i*3 + 2] = 0;
		}
	}

	void Free()
	{
		//if (line != nullptr)
		//	delete line;
		line = nullptr;
		if (pString != nullptr)
			delete pString;
		pString = nullptr;
		if (pInt != nullptr)
			delete pInt;
		pInt = nullptr;
		if (pDouble != nullptr)
			delete pDouble;
		pDouble = nullptr;
	}

	~CallbackValues()
	{
		Free();
	}
};
#pragma pack(pop)

typedef void(__stdcall *callBackFunction)(CallbackValues);

template<class Tk, class Tv>
Tv MapGetValue(std::map<Tk, Tv> map, Tk key)
{
	std::map<Tk, Tv>::iterator it = map.find(key);
	if (it != map.end())
	{
		//element found
		return it->second;
	}
	//element not found
	return nullptr;
}