using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartBILL.Commands;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class PartyChallanSearchViewModel : ViewModelBase
    {
        private readonly AppDbContext _db = new AppDbContext();
        public ObservableCollection<PartyChallanItem> PartyChallanItems { get; set; }
        public ICommand LoadDataCommand { get; set; }


        // This will be your DataGrid's ItemsSource
        public ObservableCollection<PartyChallanGroupViewModel> GroupedChallans { get; set; }


        public PartyChallanSearchViewModel()
        {
            LoadData();
            
        }

        private void LoadData()
        {
            //// Fetching PartyChallanItems and loading related data using EF6 Include
            //var items = _db.PartyChallanItems
            //               .Include("PartyChallans")
            //               .Include("Items")
            //               .Include("ProcessItems")
            //               .Include("CustomerParty")
            //               .Include("YearAccounts")
            //               .ToList();

            //PartyChallanItems = new ObservableCollection<PartyChallanItem>(items);
            // Fetching PartyChallanItems
            var items = _db.PartyChallanItems
                           .Include("PartyChallans")
                           .Include("Items")
                           .Include("ProcessItems")
                           .Include("CustomerParty")
                           .Include("YearAccounts")
                           .ToList();

            var groupedData = items
                              .GroupBy(i => i.PartyChallans.PartyChNo)
                              .Select(g => new PartyChallanGroupViewModel
                              {
                                  PartyChallanNo = g.Key.ToString(),
                                  PartyDate = g.First().PartyChallans.PartyDate,
                                  PartyName = g.First().PartyChallans.CustomerParty.CompanyP,
                                  PartyChallanItems = new ObservableCollection<PartyChallanItem>(g)
                              }).ToList();

            GroupedChallans = new ObservableCollection<PartyChallanGroupViewModel>(groupedData);

        }

    }
}
