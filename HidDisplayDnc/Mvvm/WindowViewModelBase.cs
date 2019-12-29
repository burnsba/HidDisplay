using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace HidDisplayDnc.Mvvm
{
    /// <summary>
    /// Base class for window ViewModel.
    /// </summary>
    public abstract class WindowViewModelBase : ViewModelBase
    {
        protected void CloseWindow(ICloseable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
