using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HidDisplayDnc.Mvvm;
using HidDisplayDnc.ViewModels;

namespace HidDisplayDnc.Windows
{
    /// <summary>
    /// Interaction logic for SkinConfigWindow.xaml
    /// </summary>
    public partial class SkinConfigWindow : Window, ICloseable
    {
        private SkinConfigViewModel _vm = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinConfigWindow"/> class.
        /// </summary>
        /// <param name="availableSkinViewModel">Source info.</param>
        public SkinConfigWindow(AvailableSkinViewModel availableSkinViewModel)
        {
            InitializeComponent();

            _vm = new SkinConfigViewModel(availableSkinViewModel);

            DataContext = _vm;
        }

        private void SkinConfigWindowx_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.Dispose();
        }
    }
}
