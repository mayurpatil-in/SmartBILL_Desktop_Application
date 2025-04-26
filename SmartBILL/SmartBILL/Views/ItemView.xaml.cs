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
using SmartBILL.ViewModels;
using SmartBILL.Models;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace SmartBILL.Views
{
    /// <summary>
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : UserControl
    {
        public ItemView()
        {
            InitializeComponent();
        }

     

        private void Userdatagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // get the VM
            if (!(DataContext is ItemViewModel vm)) return;

            // get the selected item from the grid (or from vm.EditingItem)
            var selected = (sender as DataGrid)?.SelectedItem as Item
                        ?? vm.EditingItem;
            if (selected == null) return;

            // copy into your form model: here we reuse CurrentItem
            vm.CurrentItem = new Item
            {
                ItemId = selected.ItemId,
                CustomerPId = selected.CustomerPId,
                ItemName = selected.ItemName,
                ItemRate = selected.ItemRate,
                PopNo = selected.PopNo,
                HsnCode = selected.HsnCode,
                CastWeight = selected.CastWeight,
                ScrapWeight = selected.ScrapWeight,
                IsActive = selected.IsActive
            };

            // and reset SelectedParty so the ComboBox jumps to that company
            vm.SelectedParty = vm.Parties
                                .FirstOrDefault(p => p.CustomerPId == selected.CustomerPId);
        }
    }
}
