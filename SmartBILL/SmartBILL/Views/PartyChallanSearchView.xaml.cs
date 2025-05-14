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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartBILL.ViewModels;

namespace SmartBILL.Views
{
    /// <summary>
    /// Interaction logic for PartyChallanSearchView.xaml
    /// </summary>
    public partial class PartyChallanSearchView : UserControl
    {
        public PartyChallanSearchView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                // Get the MainViewModel from the Window
                Window mainWindow = Window.GetWindow(this); // Change this line
                if (mainWindow != null)
                {
                    if (mainWindow is MainWindow smartBillMainWindow)
                    {
                        DataContext = new PartyChallanSearchViewModel((SmartBILL.ViewModels.MainViewModel)smartBillMainWindow.DataContext);
                    }
                    else
                    {
                        // Handle the case where the window is not of type MainWindow
                        // Perhaps log an error or set a default DataContext
                        DataContext = null; // Or some other default/error value
                    }
                }
                else
                {
                    // Handle the case where MainWindow is null (e.g., for design mode)
                    DataContext = new PartyChallanSearchViewModel(null); // Or some other default/null value
                }
            }




        }
    }
}
