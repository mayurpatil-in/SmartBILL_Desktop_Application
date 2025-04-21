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
    /// Interaction logic for CustomerPartyView.xaml
    /// </summary>
    public partial class CustomerPartyView : UserControl
    {
        public CustomerPartyView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = new CustomerPartyViewModel();
        }

        private void Partydatagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CustomerPartyViewModel vm && vm.SelectedCustParty != null)
            {
                // copy fields into your "new..." props
                vm.NewTitleP = vm.SelectedCustParty.TitleP;
                vm.NewCompanyP = vm.SelectedCustParty.CompanyP;
                vm.NewMobileP = vm.SelectedCustParty.MobileP;
                vm.NewGstNoP = vm.SelectedCustParty.GstNoP;
                vm.NewHouseP = vm.SelectedCustParty.HouseP;
                vm.NewPlaceP = vm.SelectedCustParty.PlaceP;
                vm.NewTalP = vm.SelectedCustParty.TalP;
                vm.NewDistP = vm.SelectedCustParty.DistP;
                vm.NewStateP = vm.SelectedCustParty.StateP;
                vm.NewPinCodeP = vm.SelectedCustParty.PincodeP;
                // …and so on for any other inputs you have…
            }
        }
    }
}
