using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CADController
{
    public enum Status
    {
        Success,
        Fail,

        Count,
    }

    public delegate void Callback(CallbackValues value);

    public interface IController
    {
        //==================================================
        // Работа с событиями
        //==================================================

        //отправка события
        void InputEvent(int evId);

        //движение мыши - обособленное событие
        void MouseMove(double x, double y);

        //==================================================
        // Работа с операциями
        //==================================================

        //отправка данных в операцию
        void SendInt(int value);

        //отправка данных в операцию
        void SendDouble(double value);

        //отправка данных в операцию
        void SendString(string value);
    }

    public interface IApplicationController: IController
    {

        //==================================================
        // Вспомогательные действия
        //==================================================

        //открытие сессии
        Status OpenSession(Dictionary<string, Callback> delegates);

        //закрытие сессии
        Status CloseSession();

        //назначение активного документа
        void SetActiveDocument(uint docId);

        //==================================================
        // Работа с операциями
        //==================================================

        //запуск операции
        Status Operation(int opId);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CallbackValues
    {
        public Int64 id;
        public double thickness;
        public int size;
        public string line;
        public int flag;
        public IntPtr pString, pInt, pDouble;
    }

    public interface IViewCallback
    {
        //==================================================
        // Графика
        //==================================================

        /// <summary>
        /// отрисовка геометрии
        /// </summary>
        /// <param name="value">int[] color, int thickness, double[] points, int size</param>
        void DrawGeometry(CallbackValues value);

        /// <summary>
        /// задание фона документа
        /// </summary>
        /// <param name="value">int color</param>
        void Background(CallbackValues value);

        /// <summary>
        /// показать / скрыть узлы
        /// </summary>
        /// <param name="value">int color, int thickness</param>
        void DrawNodes(CallbackValues value);

        /// <summary>
        /// показать / скрыть сетку
        /// </summary>
        /// <param name="value">int color, int thickness</param>
        void DrawMesh(CallbackValues value);

        /// <summary>
        /// задать положение камеры
        /// </summary>
        /// <param name="value">double[] pos</param>
        void SetCameraPosition(CallbackValues value);

        //==================================================
        // Текст
        //==================================================

        /// <summary>
        /// вывести текст в первую строку
        /// </summary>
        /// <param name="value">string str, int size, bool redColor</param>
        void FirstString(CallbackValues value);

        /// <summary>
        /// вывести текст во вторую строку
        /// </summary>
        /// <param name="value">string str, int size, bool redColor</param>
        void SecondString(CallbackValues value);

        /// <summary>
        /// вывести текст на консоль
        /// </summary>
        /// <param name="value">string str, int size, bool redColor</param>
        void ConsoleLog(CallbackValues value);

        //==================================================
        // Слои
        //==================================================

        /// <summary>
        /// отправить список слоев (для отображения в UI)
        /// </summary>
        /// <param name="value">int[] ids, int size, string[] names</param>
        void LayersList(CallbackValues value);

        /// <summary>
        /// отправить список видимых слоев (для отображения в UI)
        /// </summary>
        /// <param name="value">int[] ids, int size</param>
        void VisibleLayers(CallbackValues value);

        /// <summary>
        /// задать активный слой (для отображения в UI)
        /// </summary>
        /// <param name="value">int LayerId</param>
        void SetActiveLayer(CallbackValues value);

        //==================================================
        // Настройки
        //==================================================

        /// <summary>
        /// задать имя документу (для отображения в UI)
        /// </summary>
        /// <param name="value">int docId, string str, int size</param>
        void SetDocName(CallbackValues value);

        /// <summary>
        /// отправить список документов
        /// </summary>
        /// <param name="value">int[] ids, int size, string[] names</param>
        void DocsList(CallbackValues value);

        /// <summary>
        /// вернуть состояние документа (сохраненный или нет)
        /// </summary>
        /// <param name="value">int id, bool status</param>
        void SetDocState(CallbackValues value);

        /// <summary>
        /// вернуть количество всех объектов/по типам/слоев
        /// </summary>
        /// <param name="value">int[] objects, int size</param>
        void SetDocStatistics(CallbackValues value);

        /// <summary>
        /// задать активную тему (для отображения в UI)
        /// </summary>
        /// <param name="value">int themeId</param>
        void SetActiveTheme(CallbackValues value);

        /// <summary>
        /// отправить список тем (для отображения в UI)
        /// </summary>
        /// <param name="value">int[] ids, int size, string[] names</param>
        void ThemesList(CallbackValues value);

        //==================================================
        //Функции отправляются в ядро через API контроллера
        //в виде массива указателей на функции:
        //Status OpenSession(void* funcs, int size);
        //==================================================
    }
}
