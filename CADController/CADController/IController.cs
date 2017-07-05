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
}
