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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
