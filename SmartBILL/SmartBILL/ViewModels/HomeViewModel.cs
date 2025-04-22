using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartBILL.Commands;
using System.Windows.Input;
using SmartBILL.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Data.Entity;

namespace SmartBILL.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly AppDbContext _db = new AppDbContext();

        //For Companny  Name Load
        private string _activeCompanyName;

        //For Year Acoount Set
        private DateTime _newStartDate = DateTime.Today;
        private DateTime _newEndDate = DateTime.Today;
        private DateTime? _selectedStartDate;
        private DateTime? _selectedEndDate;

        public ObservableCollection<YearAccount> YearAccounts { get; }
            = new ObservableCollection<YearAccount>();

        public DateTime NewStartDate
        {
            get => _newStartDate;
            set { _newStartDate = value; OnPropertyChanged(); }
        }

        public DateTime NewEndDate
        {
            get => _newEndDate;
            set { _newEndDate = value; OnPropertyChanged(); }
        }

        public DateTime? SelectedStartDate
        {
            get => _selectedStartDate;
            private set { _selectedStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? SelectedEndDate
        {
            get => _selectedEndDate;
            private set { _selectedEndDate = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }


        #region Load Company Name
        public string ActiveCompanyName
        {
            get => _activeCompanyName;
            set { _activeCompanyName = value; OnPropertyChanged(); }
        }
        private void LoadActiveCompany()
        {
            var activeUser = _db.CustUsers.FirstOrDefault(u => u.IsActive);
            ActiveCompanyName = activeUser?.Company ?? "No active company";
        }
        #endregion

        public HomeViewModel()
        {
            // Load existing data from database
            _db.YearAccounts
               .OrderBy(y => y.YearId)
               .ToList()
               .ForEach(acc =>
               {
                   HookPropertyChanged(acc);
                   YearAccounts.Add(acc);
               });

            YearAccounts.CollectionChanged += OnCollectionChanged;
            SaveCommand = new RelayCommand(_ => AddYearAccount());

            var alreadyActive = YearAccounts.FirstOrDefault(a => a.IsActive);
            if (alreadyActive != null)
            {
                SelectedStartDate = alreadyActive.StartDate;
                SelectedEndDate = alreadyActive.EndDate;
            }

            LoadActiveCompany();
           
        }

        private void AddYearAccount()
        {
            try
            {
                var entity = new YearAccount
                {
                    StartDate = NewStartDate,
                    EndDate = NewEndDate,
                    IsActive = false
                };

                _db.YearAccounts.Add(entity);
                _db.SaveChanges();      // writes to DB & populates entity.YearId

                HookPropertyChanged(entity);
                YearAccounts.Add(entity);

                // Show success message
                MessageBox.Show(
                    "Year account saved successfully.",      // text
                    "Success",                               // caption
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // (optional) reset your inputs
                NewStartDate = DateTime.Today;
                NewEndDate = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving year account:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (YearAccount item in e.NewItems)
                    HookPropertyChanged(item);
            if (e.OldItems != null)
                foreach (YearAccount item in e.OldItems)
                    item.PropertyChanged -= YearAccount_PropertyChanged;
        }

        private void HookPropertyChanged(YearAccount item)
            => item.PropertyChanged += YearAccount_PropertyChanged;

        private void YearAccount_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(YearAccount.IsActive))
                return;

            var changed = (YearAccount)sender;
            if (changed.IsActive)
            {
                // Uncheck all others in-memory & database
                foreach (var acc in YearAccounts.Where(a => a != changed && a.IsActive))
                {
                    acc.IsActive = false;
                    _db.Entry(acc).State = EntityState.Modified;
                }

                SelectedStartDate = changed.StartDate;
                SelectedEndDate = changed.EndDate;
            }
            else
            {
                // If the active one was unchecked, clear display
                if (SelectedStartDate == changed.StartDate &&
                    SelectedEndDate == changed.EndDate)
                {
                    SelectedStartDate = SelectedEndDate = null;
                }
            }

            // Persist the single change (and any cascading unchecks)
            _db.Entry(changed).State = EntityState.Modified;
            _db.SaveChanges();
        }


    }
}
