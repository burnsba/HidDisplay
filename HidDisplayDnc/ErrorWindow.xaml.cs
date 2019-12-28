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
using HidDisplayDnc.ViewModels;

namespace HidDisplayDnc
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        private ErrorWindowViewModel _vm = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWindow"/> class.
        /// </summary>
        public ErrorWindow()
        {
            InitializeComponent();

            _vm = new ErrorWindowViewModel();

            DataContext = _vm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWindow"/> class.
        /// </summary>
        /// <param name="ErrorWindowViewModel">Source info.</param>
        public ErrorWindow(ErrorWindowViewModel vm)
        {
            InitializeComponent();

            _vm = vm;

            DataContext = _vm;
        }

        /// <summary>
        /// Ok button handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
