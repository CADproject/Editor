using System;
using System.Collections.Generic;
using CADController;

namespace CADView.Dialogs
{
    internal interface IControlledDialog
    {
        ApplicationController Controller { get; set; }
    }

    internal interface IDataDialog
    {
        List<object> Data { get; set; } 
    }

    internal interface ICallbackDialog
    {
        event EventHandler DataChanged;
        void DataProcessComplete();
    }
}
