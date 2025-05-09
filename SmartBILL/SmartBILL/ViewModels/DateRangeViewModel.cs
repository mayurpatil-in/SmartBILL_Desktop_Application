using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class DateRangeViewModel : ViewModelBase
    {
        private readonly AppDbContext _db = new AppDbContext();

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
        private DateTime _selectDate = DateTime.Now; // Optional default

        public DateTime SelectDate
        {
            get => _selectDate;
            set { _selectDate = value; OnPropertyChanged(); }
        }

        public DateRangeViewModel()
        {
            LoadActiveYearDates();
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
    }
}
