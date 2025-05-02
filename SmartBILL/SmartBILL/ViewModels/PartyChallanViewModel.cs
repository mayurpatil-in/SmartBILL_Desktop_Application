using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand AddCommand { get; }
        public PartyChallanViewModel()
        {
            LoadActiveYearDates();
            LoadCustomers();
            LoadProcess();
            AddCommand = new RelayCommand(_ => AddItem());
        }
        private void AddItem()
        {
            //try
            //{
            //    if (SelectedCustomer == null)
            //    {
            //        MessageBox.Show("Please select a customer.");
            //        return;
            //    }

            //    var activeYear = _db.YearAccounts.FirstOrDefault(y => y.IsActive);
            //    if (activeYear == null)
            //    {
            //        MessageBox.Show("Active financial year not found.");
            //        return;
            //    }

            //    var newChallan = new PartyChallan
            //    {
            //        CustomerPId = SelectedCustomer.CustomerPId,
            //        YearId = activeYear.YearId,
            //        PartyChNo = PartyChallanNumber,
            //        WorkDay = WorkingDays,
            //        PartyDate = SelectDate
            //    };

            //    _db.PartyChallans.Add(newChallan);
            //    _db.SaveChanges();

            //    MessageBox.Show("Party Challan added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error adding Party Challan:\n{ex.Message}", "Add Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            try
            {
                if (SelectedCustomer == null)
                {
                    MessageBox.Show("Please select a customer.");
                    return;
                }

                var activeYear = _db.YearAccounts.FirstOrDefault(y => y.IsActive);
                if (activeYear == null)
                {
                    MessageBox.Show("Active financial year not found.");
                    return;
                }

                // ✅ Check if PartyChallanNumber already exists for selected Customer and Year
                bool exists = _db.PartyChallans.Any(pc =>
                    pc.CustomerPId == SelectedCustomer.CustomerPId &&
                    pc.YearId == activeYear.YearId &&
                    pc.PartyChNo == PartyChallanNumber
                );

                if (exists)
                {
                    MessageBox.Show("This Party Challan Number already exists for the selected customer and year.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ✅ Add new entry
                var newChallan = new PartyChallan
                {
                    CustomerPId = SelectedCustomer.CustomerPId,
                    YearId = activeYear.YearId,
                    PartyChNo = PartyChallanNumber,
                    WorkDay = WorkingDays,
                    PartyDate = SelectDate
                };

                _db.PartyChallans.Add(newChallan);
                _db.SaveChanges();

                MessageBox.Show("Party Challan added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding Party Challan:\n{ex.Message}", "Add Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}
