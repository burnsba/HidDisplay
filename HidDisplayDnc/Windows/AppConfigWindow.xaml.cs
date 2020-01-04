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
using BurnsBac.WindowsAppToolkit.Mvvm;
using HidDisplayDnc.ViewModels;

namespace HidDisplayDnc.Windows
{
    /// <summary>
    /// Interaction logic for AppConfigWindow.xaml
    /// </summary>
    public partial class AppConfigWindow : Window, ICloseable
    {
        private AppConfigViewModel _vm = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfigWindow"/> class.
        /// </summary>
        /// <param name="availableSkinViewModel">Source info.</param>
        public AppConfigWindow(MainViewModel mainVm)
        {
            InitializeComponent();

            _vm = new AppConfigViewModel(mainVm);

            DataContext = _vm;
        }

        /// <summary>
        /// Ok button handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            _vm.SaveChanges();
            this.Close();
        }

        /// <summary>
        /// Cancel button handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
