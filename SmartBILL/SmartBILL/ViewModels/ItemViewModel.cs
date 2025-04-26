using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SmartBILL.Commands;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private readonly AppDbContext _ctx = new AppDbContext();

        //// ① The list bound to the ComboBox
        //public ObservableCollection<CustParty> ActiveParties { get; }

        //// ② The selected party’s ID 
        //private int _selectedCustomerPId;
        //public int SelectedCustomerPId
        //{
        //    get => _selectedCustomerPId;
        //    set
        //    {
        //        if (_selectedCustomerPId != value)
        //        {
        //            _selectedCustomerPId = value;
        //            OnPropertyChanged(nameof(SelectedCustomerPId));
        //        }
        //    }
        //}

        

        public ObservableCollection<CustParty> Parties { get; }
        public ObservableCollection<Item> Items { get; }
        private Item _currentItem;
        public Item CurrentItem
        {
            get => _currentItem;
            set => SetProperty(ref _currentItem, value);
        }

        private CustParty _selectedParty;
        public CustParty SelectedParty
        {
            get => _selectedParty;
            set
            {
                if (SetProperty(ref _selectedParty, value))
                    LoadItemsForSelectedParty();
            }
        }

        //private Item _editingItem;
        //public Item EditingItem
        //{
        //    get => _editingItem;
        //    set
        //    {
        //        if (SetProperty(ref _editingItem, value))
        //        {
        //            // When editing starts, populate CurrentItem with a copy
        //            CurrentItem = new Item
        //            {
        //                ItemId = _editingItem.ItemId,
        //                CustomerPId = _editingItem.CustomerPId,
        //                ItemName = _editingItem.ItemName,
        //                ItemRate = _editingItem.ItemRate,
        //                PopNo = _editingItem.PopNo,
        //                HsnCode = _editingItem.HsnCode,
        //                CastWeight = _editingItem.CastWeight,
        //                ScrapWeight = _editingItem.ScrapWeight,

        //            };
        //            SelectedParty = Parties.FirstOrDefault(p => p.CustomerPId == _editingItem.CustomerPId);
        //        }
        //    }
        //}
        private Item _editingItem;
        public Item EditingItem
        {
            get => _editingItem;
            set
            {
                if (SetProperty(ref _editingItem, value))
                {
                    // Notify WPF to re-query command states
                    CommandManager.InvalidateRequerySuggested();

                    if (_editingItem != null)
                    {
                        // Populate CurrentItem for editing
                        CurrentItem = new Item
                        {
                            ItemId = _editingItem.ItemId,
                            CustomerPId = _editingItem.CustomerPId,
                            ItemName = _editingItem.ItemName,
                            ItemRate = _editingItem.ItemRate,
                            PopNo = _editingItem.PopNo,
                            HsnCode = _editingItem.HsnCode,
                            CastWeight = _editingItem.CastWeight,
                            ScrapWeight = _editingItem.ScrapWeight,
                            IsActive = _editingItem.IsActive
                        };
                        SelectedParty = Parties.FirstOrDefault(p => p.CustomerPId == _editingItem.CustomerPId);
                    }
                }
            }
        }

        // 2) Declare the command:


        public ICommand SaveItemCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand ClearFormCommand { get; }

        public ItemViewModel()
        {
            //var actives = _db.CustPartys
            //            .Where(p => p.IsActive)
            //            .OrderBy(p => p.CompanyP)
            //            .ToList();
            //ActiveParties = new ObservableCollection<CustParty>(actives);
            // Load active companies
            Parties = new ObservableCollection<CustParty>(
                              _ctx.CustPartys.Where(p => p.IsActive).ToList()
                           );
            Items = new ObservableCollection<Item>();

            CurrentItem = new Item();
            LoadItemsForSelectedParty();
            // Create the save command
            SaveItemCommand = new RelayCommand(_ => SaveCurrentItem(),
                                               _ => CanSaveItem());
            UpdateItemCommand = new RelayCommand(_ => UpdateCurrentItem(), _ => EditingItem != null);
            DeleteItemCommand = new RelayCommand(_ => DeleteCurrentItem(), _ => EditingItem != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());
        }

        private bool CanSaveItem()
        {
            // Only allow save when a party is picked and required fields are filled
            return SelectedParty != null
                && !string.IsNullOrWhiteSpace(CurrentItem.ItemName)
                && CurrentItem.ItemRate > 0;
        }

        private void LoadItemsForSelectedParty()
        {
            //Items.Clear();
            //if (SelectedParty == null) return;

            //var list = _ctx.Items
            //               .Where(i => i.CustomerPId == SelectedParty.CustomerPId)
            //               .ToList();
            //foreach (var it in list)
            //    Items.Add(it);
            Items.Clear();
            if (SelectedParty == null) return;

            var list = _ctx.Items
                           .Where(i => i.CustomerPId == SelectedParty.CustomerPId)
                           .ToList();

            foreach (var it in list)
            {
                // subscribe once per item
                it.PropertyChanged += OnItemPropertyChanged;
                Items.Add(it);
            }
            // 5) Prepare for another new item
            CurrentItem = new Item();
            OnPropertyChanged(nameof(CurrentItem));
        }

        private void SaveCurrentItem()
        {
            try
            {
                // 1) Stamp the FK
                CurrentItem.CustomerPId = SelectedParty.CustomerPId;

                // 2) Ensure IsActive = false on new items
                CurrentItem.IsActive = true;

                // 3) Add & persist
                _ctx.Items.Add(CurrentItem);
                _ctx.SaveChanges();

                // 4) If it belongs to the selected party, add to grid
                if (CurrentItem.CustomerPId == SelectedParty.CustomerPId)
                    Items.Add(CurrentItem);

                // 5) Prepare for another new item
                CurrentItem = new Item();
                OnPropertyChanged(nameof(CurrentItem));

                // 6) Notify user of success
                MessageBox.Show(
                    "Item saved successfully.",
                    "Save Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // 7) Notify user of error
                MessageBox.Show(
                    $"Error saving item:\n{ex.Message}",
                    "Save Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void UpdateCurrentItem()
        {
            if (EditingItem == null)
                return;

            // Copy changes from CurrentItem back to the tracked entity
            EditingItem.ItemName = CurrentItem.ItemName;
            EditingItem.ItemRate = CurrentItem.ItemRate;
            EditingItem.PopNo = CurrentItem.PopNo;
            EditingItem.HsnCode = CurrentItem.HsnCode;
            EditingItem.CastWeight = CurrentItem.CastWeight;
            EditingItem.ScrapWeight = CurrentItem.ScrapWeight;
            // (keep IsActive and FK unchanged unless you need to modify them)

            try
            {
                // 1) Persist to database
                _ctx.SaveChanges();

                // 2) Refresh the UI list
                LoadItemsForSelectedParty();

                // 3) Clear the form for next entry
                ClearForm();

                // 4) Notify user of success
                MessageBox.Show(
                    "Item updated successfully.",
                    "Update Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // 5) Notify user of error
                MessageBox.Show(
                    $"Failed to update item:\n{ex.Message}",
                    "Update Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DeleteCurrentItem()
        {
            if (EditingItem == null)
                return;

            // ❶ Ask for confirmation
            var confirm = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                // ❷ Delete from EF context
                _ctx.Items.Remove(EditingItem);
                _ctx.SaveChanges();

                // ❸ Remove from UI collection
                Items.Remove(EditingItem);

                // ❹ Clear the form and refresh commands
                ClearForm();
                CommandManager.InvalidateRequerySuggested();

                // ❺ Notify success
                MessageBox.Show(
                    "Item deleted successfully.",
                    "Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // ❻ Notify error
                MessageBox.Show(
                    $"Error deleting item:\n{ex.Message}",
                    "Deletion Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void ClearForm()
        {
            // clear the input form
            CurrentItem = new Item();
            // clear any “in‐edit” state
            EditingItem = null;
            

            // if you also want to clear the grid entirely, uncomment:
            // Items.Clear();
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Item.IsActive))
            {
                // EF6 will detect the changed boolean and update the DB
                _ctx.SaveChanges();
            }
        }
    }
}
