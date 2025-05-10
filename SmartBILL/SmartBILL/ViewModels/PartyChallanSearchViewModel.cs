using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SmartBILL.Commands;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class PartyChallanSearchViewModel : DateRangeViewModel
    {
        private readonly AppDbContext _db = new AppDbContext();
        public ObservableCollection<PartyChallanItem> PartyChallanItems { get; set; }
        public ICommand LoadDataCommand { get; set; }
        
        #region Load Party
        public ObservableCollection<string> PartyNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ItemNames { get; set; } = new ObservableCollection<string>();

        private int? _selectedPartyId;
        public int? SelectedPartyId
        {
            get => _selectedPartyId;
            set
            {
                if (_selectedPartyId != value)
                {
                    _selectedPartyId = value;
                    OnPropertyChanged(nameof(SelectedPartyId));
                    LoadItemsForSelectedParty();  // Load items for this party
                    SearchChallan(null);          // Trigger search with updated ID
                }
            }
        }

        private int? _selectedItemId;
        public int? SelectedItemId
        {
            get => _selectedItemId;
            set
            {
                if (_selectedItemId != value)
                {
                    _selectedItemId = value;
                    OnPropertyChanged(nameof(SelectedItemId));
                    SearchChallan(null);  // Trigger search when the item is changed
                }
            }
        }

        private void LoadItemsForSelectedParty()
        {
            try
            {
                if (SelectedPartyId.HasValue)
                {
                    Items = new ObservableCollection<Item>(
                        _db.Items
                            .Where(i => i.CustomerPId == SelectedPartyId.Value && i.IsActive)
                            .ToList()
                    );
                }
                else
                {
                    Items = new ObservableCollection<Item>();
                }

                OnPropertyChanged(nameof(Items));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartyNames()
        {
            try
            {
                var parties = _db.CustPartys
                                 .Where(p => p.IsActive)
                                 .Select(p => p.CompanyP)
                                 .ToList();

                PartyNames = new ObservableCollection<string>(parties);
                OnPropertyChanged(nameof(PartyNames));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parties: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string _selectedItemName;
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set
            {
                if (_selectedItemName != value)
                {
                    _selectedItemName = value;
                    OnPropertyChanged(nameof(SelectedItemName));
                    SearchChallan(null);
                }
            }
        }
        #endregion

        // This will be your DataGrid's ItemsSource
        public ObservableCollection<PartyChallanGroupViewModel> GroupedChallans { get; set; }
        
        #region Search PartyChallan number
        private string _searchChallanNo;
        public string SearchChallanNo
        {
            get => _searchChallanNo;
            set
            {
                if (_searchChallanNo != value)
                {
                    _searchChallanNo = value;
                    OnPropertyChanged(nameof(SearchChallanNo));
                    SearchChallan(null); // 👈 Trigger search on every change
                }
            }
        }
        #endregion

        #region Load Party Name with Related Load Item in ComboBox
        private ObservableCollection<CustParty> _customers;
        public ObservableCollection<CustParty> Customers
        {
            get => _customers;
            set { _customers = value; OnPropertyChanged(); }
        }

        private CustParty _selectedCustomer;
        public CustParty SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    OnPropertyChanged();
                    LoadItemsForCustomer();
                }
            }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get => _items;
            set { _items = value; OnPropertyChanged(); }
        }

        private void LoadCustomers()
        {
            try
            {
                // Load only active customers
                Customers = new ObservableCollection<CustParty>(
                    _db.CustPartys
                            .Where(c => c.IsActive)
                            .ToList()
                );
            }
            catch (Exception ex)
            {
                // Handle or log error as needed
                Customers = new ObservableCollection<CustParty>();
                MessageBox.Show(
                   $"An error occurred loading customer:\n{ex.Message}",
                   "Load Error",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error
               );
            }
        }

        private void LoadItemsForCustomer()
        {
            try
            {
                if (SelectedCustomer != null)
                {
                    // Load only active items for selected active customer
                    Items = new ObservableCollection<Item>(
                        _db.Items
                                .Where(i => i.CustomerPId == SelectedCustomer.CustomerPId && i.IsActive)
                                .ToList()
                    );
                }
                else
                {
                    Items = new ObservableCollection<Item>();
                }
            }
            catch (Exception ex)
            {
                Items = new ObservableCollection<Item>();
                MessageBox.Show(
                   $"An error occurred loading Items:\n{ex.Message}",
                   "Load Error",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            }
        }
        #endregion

        #region Search Date Range (Start Date and End Date)
        
        // Hides inherited StartDate property from DateRangeViewModel
        private DateTime? _startDate;
        public new DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    SearchChallan(null); // Trigger search on date change
                }
            }
        }

        // Hides inherited EndDate property from DateRangeViewModel
        private DateTime? _endDate;
        public new DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                    SearchChallan(null); // Trigger search on date change
                }
            }
        }

        #endregion

        #region navigate
        // Add your navigation commands here
        #endregion

        public ICommand DeleteChallanItemCommand { get; set; }
        public ICommand SearchChallanCommand { get; set; }
        
        public PartyChallanSearchViewModel()
        {
            LoadData();
            LoadPartyNames();
            LoadCustomers();
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
            OnPropertyChanged(nameof(GroupedChallans));
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

        private void SearchChallan(object obj)
        {
            try
            {
                var searchTerm = SearchChallanNo?.Trim();
                int? partyId = SelectedPartyId;
                int? itemId = SelectedItemId;
                DateTime? startDate = StartDate;
                DateTime? endDate = EndDate;

                var query = _db.PartyChallanItems
                               .Include("PartyChallans")
                               .Include("Items")
                               .Include("ProcessItems")
                               .Include("CustomerParty")
                               .Include("YearAccounts")
                               .Where(i => i.YearAccounts.IsActive);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(i => i.PartyChallans.PartyChNo.ToString().Contains(searchTerm));
                }

                if (partyId.HasValue)
                {
                    query = query.Where(i => i.PartyChallans.CustomerPId == partyId.Value);
                }

                if (itemId.HasValue)
                {
                    query = query.Where(i => i.ItemId == itemId.Value);
                }

                if (startDate.HasValue)
                {
                    query = query.Where(i => i.PartyChallans.PartyDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(i => i.PartyChallans.PartyDate <= endDate.Value);
                }

                var filteredItems = query
                                    .OrderByDescending(i => i.PartyChallans.PartyDate)
                                    .ToList();

                if (!filteredItems.Any())
                {
                    GroupedChallans.Clear();
                    OnPropertyChanged(nameof(GroupedChallans));
                    MessageBox.Show("No challans found for the given filter(s).", "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var groupedData = filteredItems
                                  .GroupBy(i => i.PartyChallans.PartyChNo)
                                  .Select(g => new PartyChallanGroupViewModel
                                  {
                                      PartyChallanNo = g.Key.ToString(),
                                      PartyDate = g.First().PartyChallans.PartyDate,
                                      PartyName = g.First().PartyChallans.CustomerParty.CompanyP,
                                      PartyChallanItems = new ObservableCollection<PartyChallanItem>(g)
                                  })
                                  .OrderByDescending(g => g.PartyDate)
                                  .ToList();

                GroupedChallans = new ObservableCollection<PartyChallanGroupViewModel>(groupedData);
                OnPropertyChanged(nameof(GroupedChallans));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching Challans: {ex.Message}", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
