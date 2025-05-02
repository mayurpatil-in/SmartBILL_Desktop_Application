using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBILL.Models
{
    public class PartyChallan : INotifyPropertyChanged
    {
        private int _partychId;
        [Key]
        public int PartyChId
        {
            get => _partychId;
            set => SetProperty(ref _partychId, value);
        }

        private int _partychNo;
        public int PartyChNo
        {
            get => _partychNo;
            set => SetProperty(ref _partychNo, value);
        }

        private DateTime _partyDate;
        public DateTime PartyDate
        {
            get => _partyDate;
            set => SetProperty(ref _partyDate, value);
        }

        private int _workDay;
        public int WorkDay
        {
            get => _workDay;
            set => SetProperty(ref _workDay, value);
        }

        private int _customerPId;
        [Required]                               // make it non-nullable in the database
        public int CustomerPId
        {
            get => _customerPId;
            set => SetProperty(ref _customerPId, value);
        }

        // 1b) Add the navigation property:
        private CustParty _customerParty;
        [ForeignKey(nameof(CustomerPId))]
        public virtual CustParty CustomerParty
        {
            get => _customerParty;
            set => SetProperty(ref _customerParty, value);
        }

        private int _yearId;
        [Required]                               // make it non-nullable in the database
        public int YearId
        {
            get => _yearId;
            set => SetProperty(ref _yearId, value);
        }

        // 1b) Add the navigation property:
        private YearAccount _yearAccount;
        [ForeignKey(nameof(YearId))]
        public virtual YearAccount YearAccounts
        {
            get => _yearAccount;
            set => SetProperty(ref _yearAccount, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
