using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartBILL.ViewModels;

namespace SmartBILL.Views
{
    /// <summary>
    /// Interaction logic for ProcessView.xaml
    /// </summary>
    public partial class ProcessView : Window
    {
        public ProcessView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = new ProcessViewModel();
        }

        private void BtnProMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnProClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
