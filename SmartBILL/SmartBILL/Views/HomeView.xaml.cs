using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartBILL.Views;

namespace SmartBILL.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void BtnTabCreate_Click(object sender, RoutedEventArgs e)
        {
            // Show Create tab content, hide Select tab content
            CreateTabPanel.Visibility = Visibility.Visible;
            SelectTabPanel.Visibility = Visibility.Collapsed;

            // Change button styles to indicate active tab
            BtnTabCreate.Style = (Style)FindResource("activeTabButton");
            BtnTabSelect.Style = (Style)FindResource("tabYearButton");
        }

        private void BtnTabSelect_Click(object sender, RoutedEventArgs e)
        {
            // Show Select tab content, hide Create tab content
            SelectTabPanel.Visibility = Visibility.Visible;
            CreateTabPanel.Visibility = Visibility.Collapsed;

            // Change button styles to indicate active tab
            BtnTabSelect.Style = (Style)FindResource("activeTabButton");
            BtnTabCreate.Style = (Style)FindResource("tabYearButton");
        }

        private void BtnFiles_Click(object sender, RoutedEventArgs e)
        {
            //RegisterView registerView = new RegisterView();
            //registerView.Show();
            // Apply blur effect to the main window
            BlurEffect blur = new BlurEffect { Radius = 10 };
            this.Effect = blur;

            // Create and show the new window
            RegisterView newWindow = new RegisterView();
            /*newWindow.Owner = this; */// Set owner to ensure modal behavior
            newWindow.ShowDialog(); // Show as a modal dialog

            // Remove blur effect when the new window is closed
            this.Effect = null;
        }
    }
}
