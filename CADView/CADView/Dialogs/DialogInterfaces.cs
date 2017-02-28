using System;
using System.Collections.Generic;

namespace CADView.Dialogs
{
    internal interface IDataDialog
    {
        List<object> Data { get; set; } 
    }

    internal interface ICallbackDialog
    {
        event EventHandler DataChanged;
    }
}
