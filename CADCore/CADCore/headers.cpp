#include "headers.h"

#pragma region Callback functions names
namespace callbacks
{
	//==================================================
	// �������
	//==================================================

	/// <summary>
	/// ��������� ���������
	/// </summary>
	/// <param name="value">int[] color, int thickness, double[] points, int size</param>
	const char* DrawGeometry = "DrawGeometry";

	/// <summary>
	/// ������� ���� ���������
	/// </summary>
	/// <param name="value">int color</param>
	const char* Background = "Background";

	/// <summary>
	/// �������� / ������ ����
	/// </summary>
	/// <param name="value">int color, int thickness</param>
	const char* DrawNodes = "DrawNodes";

	/// <summary>
	/// �������� / ������ �����
	/// </summary>
	/// <param name="value">int color, int thickness</param>
	const char* DrawMesh = "DrawMesh";

	/// <summary>
	/// ������ ��������� ������
	/// </summary>
	/// <param name="value">double[] pos</param>
	const char* SetCameraPosition = "SetCameraPosition";

	//==================================================
	// �����
	//==================================================

	/// <summary>
	/// ������� ����� � ������ ������
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	const char* FirstString = "FirstString";

	/// <summary>
	/// ������� ����� �� ������ ������
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	const char* SecondString = "SecondString";

	/// <summary>
	/// ������� ����� �� �������
	/// </summary>
	/// <param name="value">string str, int size, bool redColor</param>
	const char* ConsoleLog = "ConsoleLog";

	//==================================================
	// ����
	//==================================================

	/// <summary>
	/// ��������� ������ ����� (��� ����������� � UI)
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	const char* LayersList = "LayersList";

	/// <summary>
	/// ��������� ������ ������� ����� (��� ����������� � UI)
	/// </summary>
	/// <param name="value">int[] ids, int size</param>
	const char* VisibleLayers = "VisibleLayers";

	/// <summary>
	/// ������ �������� ���� (��� ����������� � UI)
	/// </summary>
	/// <param name="value">int LayerId</param>
	const char* SetActiveLayer = "SetActiveLayer";

	//==================================================
	// ���������
	//==================================================

	/// <summary>
	/// ������ ��� ��������� (��� ����������� � UI)
	/// </summary>
	/// <param name="value">int docId, string str, int size</param>
	const char* SetDocName = "SetDocName";

	/// <summary>
	/// ��������� ������ ����������
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	const char* DocsList = "DocsList";

	/// <summary>
	/// ������� ��������� ��������� (����������� ��� ���)
	/// </summary>
	/// <param name="value">int id, bool status</param>
	const char* SetDocState = "SetDocState";

	/// <summary>
	/// ������� ���������� ���� ��������/�� �����/�����
	/// </summary>
	/// <param name="value">int[] objects, int size</param>
	const char* SetDocStatistics = "SetDocStatistics";

	/// <summary>
	/// ������ �������� ���� (��� ����������� � UI)
	/// </summary>
	/// <param name="value">int themeId</param>
	const char* SetActiveTheme = "SetActiveTheme";

	/// <summary>
	/// ��������� ������ ��� (��� ����������� � UI)
	/// </summary>
	/// <param name="value">int[] ids, int size, string[] names</param>
	const char* ThemesList = "ThemesList";
}
#pragma endregion Callback functions names