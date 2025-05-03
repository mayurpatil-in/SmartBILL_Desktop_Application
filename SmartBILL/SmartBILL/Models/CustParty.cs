using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBILL.Models
{
    public class CustParty : INotifyPropertyChanged
    {
        private int _customerPId;
        [Key]
        public int CustomerPId
        {
            get => _customerPId;
            set { if (_customerPId != value) { _customerPId = value; OnPropertyChanged(); } }
        }

        private string _titlep;
        public string TitleP
        {
            get => _titlep;
            set { if (_titlep != value) { _titlep = value; OnPropertyChanged(); } }
        }

        private string _companyp;
        public string CompanyP
        {
            get => _companyp;
            set { if (_companyp != value) { _companyp = value; OnPropertyChanged(); } }
        }

        private string _mobilep;
        public string MobileP
        {
            get => _mobilep;
            set { if (_mobilep != value) { _mobilep = value; OnPropertyChanged(); } }
        }

        private string _gstnop;
        public string GstNoP
        {
            get => _gstnop;
            set { if (_gstnop != value) { _gstnop = value; OnPropertyChanged(); } }
        }

        private string _housep;
        public string HouseP
        {
            get => _housep;
            set { if (_housep != value) { _housep = value; OnPropertyChanged(); } }
        }

        private string _placep;
        public string PlaceP
        {
            get => _placep;
            set { if (_placep != value) { _placep = value; OnPropertyChanged(); } }
        }

        private string _talp;
        public string TalP
        {
            get => _talp;
            set { if (_talp != value) { _talp = value; OnPropertyChanged(); } }
        }

        private string _distp;
        public string DistP
        {
            get => _distp;
            set { if (_distp != value) { _distp = value; OnPropertyChanged(); } }
        }

        private string _statep;
        public string StateP
        {
            get => _statep;
            set { if (_statep != value) { _statep = value; OnPropertyChanged(); } }
        }

        private string _pincodep;
        public string PincodeP
        {
            get => _pincodep;
            set { if (_pincodep != value) { _pincodep = value; OnPropertyChanged(); } }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set { if (_isActive != value) { _isActive = value; OnPropertyChanged(); } }
        }

        public virtual ICollection<Item> Items { get; set; } = new HashSet<Item>();
        public virtual ICollection<PartyChallan> PartyChallans { get; set; } = new HashSet<PartyChallan>();
        public virtual ICollection<PartyChallanItem> PartyChallanItems { get; set; } = new HashSet<PartyChallanItem>();

        public string Address
            => $"{HouseP}, {PlaceP}, {TalP}, {DistP}, {StateP} – {PincodeP}";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
