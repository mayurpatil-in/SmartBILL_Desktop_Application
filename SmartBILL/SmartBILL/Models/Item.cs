using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBILL.Models
{
    public class Item : INotifyPropertyChanged
    {
        private int _itemId;
        [Key]
        public int ItemId
        {
            get => _itemId;
            set => SetProperty(ref _itemId, value);
        }

        private string _itemName;
        [Required, MaxLength(100)]
        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        private decimal _itemRate;
        [Column(TypeName = "money")]
        public decimal ItemRate
        {
            get => _itemRate;
            set => SetProperty(ref _itemRate, value);
        }

        private string _popNo;
        [MaxLength(50)]
        public string PopNo
        {
            get => _popNo;
            set => SetProperty(ref _popNo, value);
        }

        private string _hsnCode;
        [MaxLength(50)]
        public string HsnCode
        {
            get => _hsnCode;
            set => SetProperty(ref _hsnCode, value);
        }

        private decimal _castingw;
        
        public decimal CastWeight
        {
            get => _castingw;
            set => SetProperty(ref _castingw, value);
        }

        private decimal _scrapw;

        public decimal ScrapWeight
        {
            get => _scrapw;
            set => SetProperty(ref _scrapw, value);
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
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

        public virtual ICollection<PartyChallanItem> PartyChallanItems { get; set; } = new HashSet<PartyChallanItem>();

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
