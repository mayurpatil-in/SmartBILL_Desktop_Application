using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SmartBILL.Commands;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class ProcessViewModel : ViewModelBase
    {
        private readonly AppDbContext _db = new AppDbContext();

        private ProcessItem _newProcess = new ProcessItem();
        public ProcessItem NewProcess
        {
            get => _newProcess;
            set => SetProperty(ref _newProcess, value);
        }
        private ProcessItem _selectedProcess;
        public ProcessItem SelectedProcess
        {
            get => _selectedProcess;
            set => SetProperty(ref _selectedProcess, value);
        }
        // inside CustomerPartyViewModel
        private ProcessItem _selectedCustParty;
        public ProcessItem SelectedCustParty
        {
            get => _selectedCustParty;
            set { _selectedCustParty = value; OnPropertyChanged(); }
        }
        // fire this when you’ve clicked Save and the name was empty
        private bool _showNameError;
        public bool ShowNameError
        {
            get => _showNameError;
            set => SetProperty(ref _showNameError, value);
        }

        // Collection bound to the DataGrid
        public ObservableCollection<ProcessItem> ProcessItems { get; }

        // Add button
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }


        public ProcessViewModel()
        {
            // Load from DB
            ProcessItems = new ObservableCollection<ProcessItem>(_db.ProcessItems.ToList());

            // Subscribe to any existing customer's PropertyChanged
            foreach (var c in ProcessItems)
                c.PropertyChanged += CustParty_PropertyChanged;

            foreach (var pi in ProcessItems)
                pi.PropertyChanged += Process_PropertyChanged;

            AddCommand = new RelayCommand(_ => Add(), _ => true);

            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedProcess != null);
            
            ClearCommand = new RelayCommand(_ => ClearForm(), _ => true); 
        }
        private void Process_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Auto-persist any change (e.g. IsActive toggled via checkbox)
            _db.SaveChanges();
            //var item = (ProcessItem)sender;

            //// only auto‐save when the user toggles IsActive on an existing record
            //if (e.PropertyName == nameof(ProcessItem.IsActive)
            //    && item.ProcessId != 0)      // zero means 'not yet in database'
            //{
            //    _db.SaveChanges();
            //}
        }

        private void Add()
        {
            // reset the flag
            ShowNameError = false;
            // 1) Validate the TextBox-bound property
            if (string.IsNullOrWhiteSpace(NewProcess.ProcessName))
            {
                MessageBox.Show(
                    "Please enter a process name before saving.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                ShowNameError = true;
                return;
            }

            try
            {
                // 2) Ensure it’s active
                NewProcess.IsActive = true;

                // 3) Persist
                _db.ProcessItems.Add(NewProcess);
                _db.SaveChanges();

                // 4) Hook up auto‐save on property changes
                NewProcess.PropertyChanged += Process_PropertyChanged;

                // 5) Update the UI collection
                ProcessItems.Add(NewProcess);

                // 6) Show success notification
                MessageBox.Show(
                    "Process saved successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // 7) Reset the form to a fresh, active default
                NewProcess = new ProcessItem { IsActive = true };
            }
            catch (Exception ex)
            {
                // 8) Handle any errors
                MessageBox.Show(
                    $"An error occurred while saving:\n{ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        

        private void Delete()
        {
            _db.ProcessItems.Remove(SelectedProcess);
            _db.SaveChanges();
            ProcessItems.Remove(SelectedProcess);
            SelectedProcess = null;
        }

        private void ClearForm()
        {
            // Either reset the existing NewProcess…
            NewProcess.ProcessName = string.Empty;

            // …or if you want to clear everything (e.g. default values):
            // NewProcess = new ProcessItem();
            //
            // If you do that, make sure your NewProcess setter
            // raises PropertyChanged (it does if you used SetProperty).
        }
        private void CustParty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only persist when the Active checkbox is toggled
            if (e.PropertyName == nameof(ProcessItem.IsActive))
                _db.SaveChanges();
        }
    }
}
