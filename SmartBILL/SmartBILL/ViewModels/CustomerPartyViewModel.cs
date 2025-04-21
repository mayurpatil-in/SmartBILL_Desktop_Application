using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBILL.Commands;
using SmartBILL.Models;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace SmartBILL.ViewModels
{
    public class CustomerPartyViewModel : ViewModelBase
    {
        private readonly AppDbContext _db = new AppDbContext();

        // Backing for the two input TextBoxes
        private string _newTitleP;
        private string _newCompanyP;
        private string _newMobileP;
        private string _newGstNoP;
        private string _newHouseP;
        private string _newPlaceP;
        private string _newTalP;
        private string _newDistP;
        private string _newStateP;
        private string _newPinCodeP;

        public string NewTitleP
        {
            get => _newTitleP;
            set { _newTitleP = value; OnPropertyChanged(); }
        }

        public string NewCompanyP
        {
            get => _newCompanyP;
            set { _newCompanyP = value; OnPropertyChanged(); }
        }

        public string NewMobileP
        {
            get => _newMobileP;
            set { _newMobileP = value; OnPropertyChanged(); }
        }

        public string NewGstNoP
        {
            get => _newGstNoP;
            set { _newGstNoP = value; OnPropertyChanged(); }
        }

        public string NewHouseP
        {
            get => _newHouseP;
            set { _newHouseP = value; OnPropertyChanged(); }
        }

        public string NewPlaceP
        {
            get => _newPlaceP;
            set { _newPlaceP = value; OnPropertyChanged(); }
        }

        public string NewTalP
        {
            get => _newTalP;
            set { _newTalP = value; OnPropertyChanged(); }
        }

        public string NewDistP
        {
            get => _newDistP;
            set { _newDistP = value; OnPropertyChanged(); }
        }

        public string NewStateP
        {
            get => _newStateP;
            set { _newStateP = value; OnPropertyChanged(); }
        }

        public string NewPinCodeP
        {
            get => _newPinCodeP;
            set { _newPinCodeP = value; OnPropertyChanged(); }
        }

        // inside CustomerPartyViewModel
        private CustParty _selectedCustParty;
        public CustParty SelectedCustParty
        {
            get => _selectedCustParty;
            set { _selectedCustParty = value; OnPropertyChanged(); }
        }

        // Collection bound to the DataGrid
        public ObservableCollection<CustParty> CustPartys { get; }

        // Add button
        public ICommand AddCommandP { get; }
        public ICommand UpdateCommandP { get; }
        public ICommand DeleteCommandP { get; }


        public CustomerPartyViewModel()
        {
            // Load from DB
            CustPartys = new ObservableCollection<CustParty>(_db.CustPartys.ToList());

            // Subscribe to any existing customer's PropertyChanged
            foreach (var c in CustPartys)
                c.PropertyChanged += CustParty_PropertyChanged;

            // Create AddCommand: only enabled when NewName is non-empty
            AddCommandP = new RelayCommand(_ => AddCustomerP(),
                                          _ => !string.IsNullOrWhiteSpace(NewTitleP));

            // Update: enabled only when an item is selected
            UpdateCommandP = new RelayCommand(
                _ => UpdateCustomerP(),
                _ => SelectedCustParty != null
            );

            DeleteCommandP = new RelayCommand(
                _ => DeleteCustomerP(),
                _ => SelectedCustParty != null
            );
        }

        private bool ValidateInputs(out string missingFields)
        {
            var missing = new[]
            {
                (NewTitleP,   "Title"),
                (NewCompanyP, "Company"),
                (NewMobileP,  "Mobile"),
                (NewGstNoP,   "GST No."),
                (NewHouseP,   "House"),
                (NewPlaceP,   "Place"),
                (NewTalP,     "Tal"),
                (NewDistP,    "Dist"),
                (NewStateP,   "State"),
                (NewPinCodeP, "Pin Code")
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

        private void AddCustomerP()
        {
            // validation
            if (!ValidateInputs(out var msg))
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var cust = new CustParty
                {
                    TitleP = NewTitleP.Trim(),
                    CompanyP = NewCompanyP.Trim(),
                    MobileP = NewMobileP.Trim(),
                    GstNoP = NewGstNoP.Trim(),
                    HouseP = NewHouseP.Trim(),
                    PlaceP = NewPlaceP.Trim(),
                    TalP = NewTalP.Trim(),
                    DistP = NewDistP.Trim(),
                    StateP = NewStateP.Trim(),
                    PincodeP = NewPinCodeP.Trim(),
                    IsActive = false
                };

                _db.CustPartys.Add(cust);
                _db.SaveChanges();

                cust.PropertyChanged += CustParty_PropertyChanged;
                CustPartys.Add(cust);
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

        private void UpdateCustomerP()
        {
            if (SelectedCustParty == null) return;

            // validation
            if (!ValidateInputs(out var msg))
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var existing = _db.CustPartys.Find(SelectedCustParty.CustomerPId);
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

                existing.TitleP = NewTitleP.Trim();
                existing.CompanyP = NewCompanyP.Trim();
                existing.MobileP = NewMobileP.Trim();
                existing.GstNoP = NewGstNoP.Trim();
                existing.HouseP = NewHouseP.Trim();
                existing.PlaceP = NewPlaceP.Trim();
                existing.TalP = NewTalP.Trim();
                existing.DistP = NewDistP.Trim();
                existing.StateP = NewStateP.Trim();
                existing.PincodeP = NewPinCodeP.Trim();

                _db.Entry(existing).State = EntityState.Modified;
                _db.SaveChanges();

                var idx = CustPartys.IndexOf(SelectedCustParty);
                CustPartys[idx] = existing;

                SelectedCustParty = null;
                ClearInputs();
                CollectionViewSource.GetDefaultView(CustPartys).Refresh();
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

        private void DeleteCustomerP()
        {
            if (SelectedCustParty == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this customer?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var existing = _db.CustPartys.Find(SelectedCustParty.CustomerPId);
                if (existing != null)
                {
                    _db.CustPartys.Remove(existing);
                    _db.SaveChanges();
                }

                CustPartys.Remove(SelectedCustParty);
                SelectedCustParty = null;
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
            NewTitleP = NewCompanyP = NewMobileP = NewGstNoP =
            NewHouseP = NewPlaceP = NewTalP = NewDistP = NewStateP = NewPinCodeP = string.Empty;
        }

        // Whenever a Customer's property changes...
        private void CustParty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only persist when the Active checkbox is toggled
            if (e.PropertyName == nameof(CustParty.IsActive))
                _db.SaveChanges();
        }

        
    }
}
