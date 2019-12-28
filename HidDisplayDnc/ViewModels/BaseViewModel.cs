using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// Base mvvm view model.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises property changed event.
        /// </summary>
        /// <param name="name">Name of source property.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
