using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using SmartBILL.Commands;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class CustomerUserViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _db = new AppDbContext();

        // Backing for the two input TextBoxes
        private string _newTitle;
        private string _newCompany;
        private string _newMobile;
        private string _newGstNo;
        private string _newHouse;
        private string _newPlace;
        private string _newTal;
        private string _newDist;
        private string _newState;
        private string _newPinCode;

        public string NewTitle
        {
            get => _newTitle;
            set { _newTitle = value; OnPropertyChanged(); }
        }

        public string NewCompany
        {
            get => _newCompany;
            set { _newCompany = value; OnPropertyChanged(); }
        }

        public string NewMobile
        {
            get => _newMobile;
            set { _newMobile = value; OnPropertyChanged(); }
        }

        public string NewGstNo
        {
            get => _newGstNo;
            set { _newGstNo = value; OnPropertyChanged(); }
        }

        public string NewHouse
        {
            get => _newHouse;
            set { _newHouse = value; OnPropertyChanged(); }
        }

        public string NewPlace
        {
            get => _newPlace;
            set { _newPlace = value; OnPropertyChanged(); }
        }

        public string NewTal
        {
            get => _newTal;
            set { _newTal = value; OnPropertyChanged(); }
        }

        public string NewDist
        {
            get => _newDist;
            set { _newDist = value; OnPropertyChanged(); }
        }

        public string NewState
        {
            get => _newState;
            set { _newState = value; OnPropertyChanged(); }
        }

        public string NewPinCode
        {
            get => _newPinCode;
            set { _newPinCode = value; OnPropertyChanged(); }
        }

        // inside CustomerUserViewModel
        private CustUser _selectedCustUser;
        public CustUser SelectedCustUser
        {
            get => _selectedCustUser;
            set { _selectedCustUser = value; OnPropertyChanged(); }
        }

        // Collection bound to the DataGrid
        public ObservableCollection<CustUser> CustUsers { get; }

        // Add button
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }


        public CustomerUserViewModel()
        {
            // Load from DB
            CustUsers = new ObservableCollection<CustUser>(_db.CustUsers.ToList());

            // Subscribe to any existing customer's PropertyChanged
            foreach (var c in CustUsers)
                c.PropertyChanged += CustUser_PropertyChanged;

            // Create AddCommand: only enabled when NewName is non-empty
            AddCommand = new RelayCommand(_ => AddCustomer(),
                                          _ => !string.IsNullOrWhiteSpace(NewTitle));

            // Update: enabled only when an item is selected
            UpdateCommand = new RelayCommand(
                _ => UpdateCustomer(),
                _ => SelectedCustUser != null
            );

            DeleteCommand = new RelayCommand(
                _ => DeleteCustomer(),
                _ => SelectedCustUser != null
            );
        }

        private bool ValidateInputs(out string missingFields)
        {
            var missing = new[]
            {
                (NewTitle,   "Title"),
                (NewCompany, "Company"),
                (NewMobile,  "Mobile"),
                (NewGstNo,   "GST No."),
                (NewHouse,   "House"),
                (NewPlace,   "Place"),
                (NewTal,     "Tal"),
                (NewDist,    "Dist"),
                (NewState,   "State"),
                (NewPinCode, "Pin Code")
            }
            .Where(t => string.IsNullOrWhiteSpace(t.Item1))
            .Select(t => t.Item2)
            .ToList();

            if (missing.Any())
            {
                missingFields = "Please fill in the following fields:\n" +
                                string.Join("\n", missing.Select(f => "• " + f));
                return false;
            }

            missingFields = null;
            return true;
        }

        private void AddCustomer()
        {
            // validation
            if (!ValidateInputs(out var msg))
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var cust = new CustUser
                {
                    Title = NewTitle.Trim(),
                    Company = NewCompany.Trim(),
                    Mobile = NewMobile.Trim(),
                    GstNo = NewGstNo.Trim(),
                    House = NewHouse.Trim(),
                    Place = NewPlace.Trim(),
                    Tal = NewTal.Trim(),
                    Dist = NewDist.Trim(),
                    State = NewState.Trim(),
                    Pincode = NewPinCode.Trim(),
                    IsActive = false
                };

                _db.CustUsers.Add(cust);
                _db.SaveChanges();

                cust.PropertyChanged += CustUser_PropertyChanged;
                CustUsers.Add(cust);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error adding customer:\n{ex.Message}",
                    "Add Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void UpdateCustomer()
        {
            if (SelectedCustUser == null) return;

            // validation
            if (!ValidateInputs(out var msg))
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var existing = _db.CustUsers.Find(SelectedCustUser.CustomerId);
                if (existing == null)
                {
                    MessageBox.Show(
                        "Customer not found in database.",
                        "Update Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                existing.Title = NewTitle.Trim();
                existing.Company = NewCompany.Trim();
                existing.Mobile = NewMobile.Trim();
                existing.GstNo = NewGstNo.Trim();
                existing.House = NewHouse.Trim();
                existing.Place = NewPlace.Trim();
                existing.Tal = NewTal.Trim();
                existing.Dist = NewDist.Trim();
                existing.State = NewState.Trim();
                existing.Pincode = NewPinCode.Trim();

                _db.Entry(existing).State = EntityState.Modified;
                _db.SaveChanges();

                var idx = CustUsers.IndexOf(SelectedCustUser);
                CustUsers[idx] = existing;

                SelectedCustUser = null;
                ClearInputs();
                CollectionViewSource.GetDefaultView(CustUsers).Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating customer:\n{ex.Message}",
                    "Update Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void DeleteCustomer()
        {
            if (SelectedCustUser == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this customer?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var existing = _db.CustUsers.Find(SelectedCustUser.CustomerId);
                if (existing != null)
                {
                    _db.CustUsers.Remove(existing);
                    _db.SaveChanges();
                }

                CustUsers.Remove(SelectedCustUser);
                SelectedCustUser = null;
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error deleting customer:\n{ex.Message}",
                    "Delete Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ClearInputs()
        {
            NewTitle = NewCompany = NewMobile = NewGstNo =
            NewHouse = NewPlace = NewTal = NewDist = NewState = NewPinCode = string.Empty;
        }

        // Whenever a Customer's property changes...
        private void CustUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only persist when the Active checkbox is toggled
            if (e.PropertyName == nameof(CustUser.IsActive))
                _db.SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
