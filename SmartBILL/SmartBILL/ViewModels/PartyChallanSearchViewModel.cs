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

        
        public ICommand DeleteChallanItemCommand { get; set; }
        public PartyChallanSearchViewModel()
        {
            LoadData();
            DeleteChallanItemCommand = new RelayCommand(DeleteGroupedChallan);

        }

        private void LoadData()
        {
            // Fetching PartyChallanItems with related entities and filtering YearAccounts.IsActive == true
            var items = _db.PartyChallanItems
                           .Include("PartyChallans")
                           .Include("Items")
                           .Include("ProcessItems")
                           .Include("CustomerParty")
                           .Include("YearAccounts")
                           .Where(i => i.YearAccounts.IsActive) // Filter only active YearAccounts
                           .OrderByDescending(i => i.PartyChallans.PartyDate) // Sort by PartyDate descending
                           .ToList();

            // Grouping by PartyChNo and creating the ViewModel
            var groupedData = items
                              .GroupBy(i => i.PartyChallans.PartyChNo)
                              .Select(g => new PartyChallanGroupViewModel
                              {
                                  PartyChallanNo = g.Key.ToString(),
                                  PartyDate = g.First().PartyChallans.PartyDate,
                                  PartyName = g.First().PartyChallans.CustomerParty.CompanyP,
                                  PartyChallanItems = new ObservableCollection<PartyChallanItem>(g)
                              })
                              .OrderByDescending(g => g.PartyDate) // Ensure groups are also in descending order
                              .ToList();

            GroupedChallans = new ObservableCollection<PartyChallanGroupViewModel>(groupedData);
        }


        private void DeleteGroupedChallan(object parameter)
        {
            if (parameter is PartyChallanGroupViewModel groupToDelete)
            {
                // Confirm (optional)
                var result = System.Windows.MessageBox.Show($"Are you sure you want to delete Challan {groupToDelete.PartyChallanNo}?",
                                                            "Confirm Delete",
                                                            System.Windows.MessageBoxButton.YesNo,
                                                            System.Windows.MessageBoxImage.Warning);
                if (result != System.Windows.MessageBoxResult.Yes)
                    return;

                // Delete all associated PartyChallanItems from DB
                foreach (var item in groupToDelete.PartyChallanItems)
                {
                    _db.PartyChallanItems.Remove(item);
                }

                _db.SaveChanges();

                // Remove group from UI
                GroupedChallans.Remove(groupToDelete);
            }
        }

    }
}
