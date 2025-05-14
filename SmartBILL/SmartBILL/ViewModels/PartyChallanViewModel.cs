using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
using CrystalDecisions.ReportAppServer;
using SmartBILL.Commands;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class PartyChallanViewModel : ViewModelBase
    {
        private readonly AppDbContext _db = new AppDbContext();


        #region DatePicker Load Date
        private DateTime? _startDate;
        private DateTime? _endDate;

        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        private void LoadActiveYearDates()
        {
            try
            {
                var activeYear = _db.YearAccounts.FirstOrDefault(y => y.IsActive);
                if (activeYear != null)
                {
                    StartDate = activeYear.StartDate;
                    EndDate = activeYear.EndDate;
                }
                else
                {
                    StartDate = null;
                    EndDate = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while load date:\n{ex.Message}",
                    "Load DatePicker Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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

        #region Load Process Name in Combobox
        private ObservableCollection<ProcessItem> _processitem;
        public ObservableCollection<ProcessItem> ProcessItems
        {
            get => _processitem;
            set { _processitem = value; OnPropertyChanged(); }
        }
        private void LoadProcess()
        {
            try
            {
                // Load only active customers
                ProcessItems = new ObservableCollection<ProcessItem>(
                    _db.ProcessItems
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
        #endregion

        #region PartyChallan Input Properties

        private int _partyChallanNumber;
        public int PartyChallanNumber
        {
            get => _partyChallanNumber;
            set { _partyChallanNumber = value; OnPropertyChanged(); }
        }

        private int _workingDays = 180; // default value
        public int WorkingDays
        {
            get => _workingDays;
            set { _workingDays = value; OnPropertyChanged(); }
        }

        private DateTime _selectDate = DateTime.Now; // Optional default

        public DateTime SelectDate
        {
            get => _selectDate;
            set { _selectDate = value; OnPropertyChanged(); }
        }
        #endregion

        #region Add PartyChallanItems And Save Party Challan

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private ProcessItem _selectedProcessItem;
        public ProcessItem SelectedProcessItem
        {
            get => _selectedProcessItem;
            set { _selectedProcessItem = value; OnPropertyChanged(); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PartyChallanItem> PartyChallanItems { get; set; } = new ObservableCollection<PartyChallanItem>();

        private void AddItemToChallan()
        {
            // Basic validation
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an item.");
                return;
            }

            if (SelectedProcessItem == null)
            {
                MessageBox.Show("Please select a process for the item.");
                return;
            }

            if (Quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0.");
                return;
            }

            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer before adding items.");
                return;
            }

            // Ensure active financial year exists
            var activeYear = _db.YearAccounts.FirstOrDefault(y => y.IsActive);
            if (activeYear == null)
            {
                MessageBox.Show("Active financial year not found.");
                return;
            }

            // Check for existing item-process combination to avoid duplication
            var existingItem = PartyChallanItems.FirstOrDefault(i =>
                i.ItemId == SelectedItem.ItemId &&
                i.ProcessId == SelectedProcessItem.ProcessId);

            if (existingItem != null)
            {
                existingItem.Quantity += Quantity;
                OnPropertyChanged(nameof(PartyChallanItems)); // Refresh UI
            }
            else
            {
                var newItem = new PartyChallanItem
                {
                    ItemId = SelectedItem.ItemId,
                    ProcessId = SelectedProcessItem.ProcessId,
                    Quantity = Quantity,
                    CustomerPId = SelectedCustomer.CustomerPId,
                    YearId = activeYear.YearId,
                    Items = SelectedItem,
                    ProcessItems = SelectedProcessItem
                };

                PartyChallanItems.Add(newItem);
            }

            // Reset form fields
            SelectedItem = null;
            SelectedProcessItem = null;
            Quantity = 0;

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(SelectedProcessItem));
            OnPropertyChanged(nameof(Quantity));
        }
        private void SaveChallan()
        {
            if (SelectedCustomer == null || PartyChallanNumber <= 0)
            {
                MessageBox.Show("Customer and valid Challan number are required.");
                return;
            }

            var activeYear = _db.YearAccounts.FirstOrDefault(y => y.IsActive);
            if (activeYear == null)
            {
                MessageBox.Show("Active financial year not found.");
                return;
            }

            bool exists = _db.PartyChallans.Any(pc =>
                pc.CustomerPId == SelectedCustomer.CustomerPId &&
                pc.YearId == activeYear.YearId &&
                pc.PartyChNo == PartyChallanNumber);

            if (exists)
            {
                MessageBox.Show("Challan Number already exists for this customer and year.");
                return;
            }

            var newChallan = new PartyChallan
            {
                CustomerPId = SelectedCustomer.CustomerPId,
                YearId = activeYear.YearId,
                PartyChNo = PartyChallanNumber,
                WorkDay = WorkingDays,
                PartyDate = SelectDate,
                PartyChallanItems = PartyChallanItems.ToList()
            };

            _db.PartyChallans.Add(newChallan);
            _db.SaveChanges();

            MessageBox.Show("Challan saved successfully!");

            // Clear form
            PartyChallanItems.Clear();

            WorkingDays = 0;

            OnPropertyChanged(nameof(PartyChallanItems));
            OnPropertyChanged(nameof(PartyChallanNumber));
            OnPropertyChanged(nameof(WorkingDays));
        }
        #endregion

        #region Remove PartyChallanItem
        private void RemoveSelectedChallanItem(PartyChallanItem selectedItem)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                PartyChallanItems.Remove(selectedItem);
                OnPropertyChanged(nameof(PartyChallanItems));
            }
        }

        #endregion
        public ICommand AddItemToChallanCommand { get; }
        public ICommand SaveChallanCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RemoveSelectedItemCommand { get; }
        private PartyChallan _challan; // This will hold the selected data
        
        public PartyChallanViewModel()
        {
            
            LoadActiveYearDates();
            LoadCustomers();
            LoadProcess();
            
            AddItemToChallanCommand = new RelayCommand(_ => AddItemToChallan());
            SaveChallanCommand = new RelayCommand(_ => SaveChallan());
            //DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedProcess != null);
            RemoveSelectedItemCommand = new RelayCommand<PartyChallanItem>(RemoveSelectedChallanItem);
            
        }

        //public void LoadChallanData(PartyChallan challan)
        //{
        //    if (challan == null)
        //    {
        //        MessageBox.Show("Invalid Challan data.");
        //        return;
        //    }

        //    // Load the PartyChallan by its primary key (PartyChId).
        //    var fullChallan = _db.PartyChallans.FirstOrDefault(c => c.PartyChId == challan.PartyChId);
        //    if (fullChallan == null)
        //    {
        //        MessageBox.Show("Challan not found.");
        //        return;
        //    }

        //    // Assign values from the fullChallan.
        //    SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerPId == fullChallan.CustomerPId);
        //    PartyChallanNumber = fullChallan.PartyChNo;
        //    SelectDate = fullChallan.PartyDate;
        //    WorkingDays = fullChallan.WorkDay;

        //    PartyChallanItems.Clear();

        //    foreach (var item in fullChallan.PartyChallanItems)
        //    {
        //        var fullItem = _db.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
        //        var fullProcess = _db.ProcessItems.FirstOrDefault(p => p.ProcessId == item.ProcessId);

        //        PartyChallanItems.Add(new PartyChallanItem
        //        {
        //            ItemId = item.ItemId,
        //            ProcessId = item.ProcessId,
        //            Quantity = item.Quantity,
        //            Items = fullItem,
        //            ProcessItems = fullProcess,
        //            CustomerPId = fullChallan.CustomerPId,
        //            YearId = fullChallan.YearId
        //        });
        //    }

        //    OnPropertyChanged(nameof(PartyChallanItems));
        //}

        




    }
}
