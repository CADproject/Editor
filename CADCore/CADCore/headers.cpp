#include "headers.h"

#pragma region Callback functions names
namespace callbacks
{
	//==================================================
	// Графика
	//==================================================

	/// <summary>
	/// отрисовка геометрии
	/// </summary>
	/// <param name="value">int[] color, int thickness, double[] points, int size</param>
	const char* DrawGeometry = "DrawGeometry";

	/// <summary>
	/// задание фона документа
	/// </summary>
	/// <param name="value">int color</param>
	const char* Background = "Background";

	/// <summary>
	/// показать / скрыть узлы
	/// </summary>
	/// <param name="value">int color, int thickness</param>
	const char* DrawNodes = "DrawNodes";

	/// <summary>
	/// показать / скрыть сетку
	/// </summary>
	/// <param name="value">int color, int thickness</param>
	const char* DrawMesh = "DrawMesh";

	/// <summary>
	/// задать положение камеры
	/// </summary>
	/// <param name="value">double[] pos</param>
	const char* SetCameraPosition = "SetCameraPosition";

	//==================================================
	// Текст
	//==================================================

	/// <summary>
	/// вывести текст в первую строку
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	const char* FirstString = "FirstString";

	/// <summary>
	/// вывести текст во вторую строку
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	const char* SecondString = "SecondString";

	/// <summary>
	/// вывести текст на консоль
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	const char* ConsoleLog = "ConsoleLog";

	//==================================================
	// Слои
	//==================================================

	/// <summary>
	/// отправить список слоев (для отображения в UI)
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	const char* LayersList = "LayersList";

	/// <summary>
	/// отправить список видимых слоев (для отображения в UI)
	/// </summary>
	/// <param name="value">int[] ids, int size</param>
	const char* VisibleLayers = "VisibleLayers";

	/// <summary>
	/// задать активный слой (для отображения в UI)
	/// </summary>
	/// <param name="value">int LayerId</param>
	const char* SetActiveLayer = "SetActiveLayer";

	//==================================================
	// Настройки
	//==================================================

	/// <summary>
	/// задать имя документу (для отображения в UI)
	/// </summary>
	/// <param name="value">int docId, string str, int size</param>
	const char* SetDocName = "SetDocName";

	/// <summary>
	/// отправить список документов
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	const char* DocsList = "DocsList";

	/// <summary>
	/// вернуть состояние документа (сохраненный или нет)
	/// </summary>
	/// <param name="value">int id, bool status</param>
	const char* SetDocState = "SetDocState";

	/// <summary>
	/// вернуть количество всех объектов/по типам/слоев
	/// </summary>
	/// <param name="value">int[] objects, int size</param>
	const char* SetDocStatistics = "SetDocStatistics";

	/// <summary>
	/// задать активную тему (для отображения в UI)
	/// </summary>
	/// <param name="value">int themeId</param>
	const char* SetActiveTheme = "SetActiveTheme";

	/// <summary>
	/// отправить список тем (для отображения в UI)
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	const char* ThemesList = "ThemesList";
}
#pragma endregion Callback functions names