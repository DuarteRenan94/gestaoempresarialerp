using GestaoEmpresarialERP.Ui;
using GestaoEmpresarialERP.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private static Messenger _messenger = null!;
        public MainWindow()
        {
            InitializeComponent();
            _messenger = new(this.Content.XamlRoot);
            PaginasFrame.Navigate(typeof(Pdv));
        }

        public static Messenger GetMessengerInstance()
        {
            return _messenger;
        }
    }
}
