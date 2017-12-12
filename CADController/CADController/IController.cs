using System;

namespace CADController
{
    public enum Status
    {
        Success,
        Fail,

        Count,
    }

    public interface IController
    {
        //==================================================
        // Вспомогательные действия
        //==================================================

        //открытие сессии
        Status OpenSession(params Delegate[] delegates);

        //закрытие сессии
        Status CloseSession();

        //назначение активного документа
        void SetActiveDocument(uint docId);

        //==================================================
        // Работа с событиями
        //==================================================

        //отправка события
        void Event(int evId);

        //движение мыши - обособленное событие
        void MouseMove(double x, double y);

        //==================================================
        // Работа с операциями
        //==================================================

        //запуск операции
        Status Operation(int opId);

        //отправка данных в операцию
        void SendInt(int value);

        //отправка данных в операцию
        void SendDouble(double value);

        //отправка данных в операцию
        void SendString(string value);
    }

    public interface IViewCallback
    {
        //заметка: все функции должны быть приведены к одному виду, чтобы их можно было передать в виде массива делегатов
        //таким образом, нам требуется структура вида:
        /*
        [StructLayout(LayoutKind.Sequential)]
        public struct CallbackValues
        {
            public int p1, p2, p3;
            public string p4;
            public bool p5;
            public IntPtr p6, p7, p8, p9, p10; //string[], int[], int[], int[], double[]
        }
        */

        //==================================================
        // Графика
        //==================================================

        //отрисовка геометрии
        void DrawGeometry(int color, int thickness, double[] points, int size);

        //задание фона документа
        void Background(int color);

        //показать / скрыть узлы
        void DrawNodes(int color, int thickness);

        //показать / скрыть сетку
        void DrawMesh(int color, int thickness);

        //задать положение камеры
        void SetCameraPosition(double[] pos);

        //==================================================
        // Текст
        //==================================================

        //вывести текст в первую строку
        void FirstString(string str, int size, bool redColor);

        //вывести текст во вторую строку
        void SecondString(string str, int size, bool redColor);

        //вывести текст на консоль
        void ConsoleLog(string str, int size, bool redColor);

        //==================================================
        // Слои
        //==================================================

        //отправить список слоев (для отображения в UI)
        void LayersList(int[] ids, int[] size, string[] names);

        //отправить список видимых слоев (для отображения в UI)
        void VisibleLayers(int[] ids, int[] size);

        //задать активный слой (для отображения в UI)
        void SetActiveLayer(int LayerId);

        //==================================================
        // Настройки
        //==================================================

        //задать имя документу (для отображения в UI)
        void SetDocName(int docId, string str, int size);

        //отправить список документов
        void DocsList(int[] ids, int[] size, string[] names);

        //вернуть состояние документа (сохраненный или нет)
        void SetDocState(int id, bool status);

        //вернуть количество всех объектов/по типам/слоев
        void SetDocStatistics(int[] objects, int size);

        //задать активную тему (для отображения в UI)
        void SetActiveTheme(int themeId);

        //отправить список тем (для отображения в UI)
        void ThemesList(int[] ids, int[] size, string[] names);

        //==================================================
        //Функции отправляются в ядро через API контроллера
        //в виде массива указателей на функции:
        //Status OpenSession(void* funcs, int size);
        //==================================================
    }
}
