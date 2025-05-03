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
    public class PartyChallanItem : INotifyPropertyChanged
    {
        private int _partyChallanItemId;
        [Key]
        public int PartyChallanItemId
        {
            get => _partyChallanItemId;
            set => SetProperty(ref _partyChallanItemId, value);
        }

        private int _partychId;
        [Required]                               // make it non-nullable in the database
        public int PartychId
        {
            get => _partychId;
            set => SetProperty(ref _partychId, value);
        }

        // 1b) Add the navigation property:
        private PartyChallan _partyChallan;
        [ForeignKey(nameof(PartychId))]
        public virtual PartyChallan PartyChallans
        {
            get => _partyChallan;
            set => SetProperty(ref _partyChallan, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private int _itemId;
        [Required]                               // make it non-nullable in the database
        public int ItemId
        {
            get => _itemId;
            set => SetProperty(ref _itemId, value);
        }

        // 1b) Add the navigation property:
        private Item _item;
        [ForeignKey(nameof(ItemId))]
        public virtual Item Items
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private int _processId;
        [Required]                               // make it non-nullable in the database
        public int ProcessId
        {
            get => _processId;
            set => SetProperty(ref _processId, value);
        }

        // 1b) Add the navigation property:
        private ProcessItem _processItem;
        [ForeignKey(nameof(ProcessId))]
        public virtual ProcessItem ProcessItems
        {
            get => _processItem;
            set => SetProperty(ref _processItem, value);
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
