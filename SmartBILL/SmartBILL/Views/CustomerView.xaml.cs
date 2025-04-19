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
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = new CustomerUserViewModel();
        }

        private void Userdatagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CustomerUserViewModel vm && vm.SelectedCustUser != null)
            {
                // copy fields into your "new..." props
                vm.NewTitle = vm.SelectedCustUser.Title;
                vm.NewCompany = vm.SelectedCustUser.Company;
                vm.NewMobile = vm.SelectedCustUser.Mobile;
                vm.NewGstNo = vm.SelectedCustUser.GstNo;
                vm.NewHouse = vm.SelectedCustUser.House;
                vm.NewPlace = vm.SelectedCustUser.Place;
                vm.NewTal= vm.SelectedCustUser.Tal;
                vm.NewDist = vm.SelectedCustUser.Dist;
                vm.NewState = vm.SelectedCustUser.State;
                vm.NewPinCode = vm.SelectedCustUser.Pincode;
                // …and so on for any other inputs you have…
            }
        }
    }
}
