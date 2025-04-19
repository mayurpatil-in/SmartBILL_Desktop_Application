using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBILL.Models
{
    public class CustUser : INotifyPropertyChanged
    {
        private int _customerId;
        [Key]
        public int CustomerId
        {
            get => _customerId;
            set { if (_customerId != value) { _customerId = value; OnPropertyChanged(); } }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set { if (_title != value) { _title = value; OnPropertyChanged(); } }
        }

        private string _company;
        public string Company
        {
            get => _company;
            set { if (_company != value) { _company = value; OnPropertyChanged(); } }
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set { if (_mobile != value) { _mobile = value; OnPropertyChanged(); } }
        }

        private string _gstno;
        public string GstNo
        {
            get => _gstno;
            set { if (_gstno != value) { _gstno = value; OnPropertyChanged(); } }
        }

        private string _house;
        public string House
        {
            get => _house;
            set { if (_house != value) { _house = value; OnPropertyChanged(); } }
        }

        private string _place;
        public string Place
        {
            get => _place;
            set { if (_place != value) { _place = value; OnPropertyChanged(); } }
        }

        private string _tal;
        public string Tal
        {
            get => _tal;
            set { if (_tal != value) { _tal = value; OnPropertyChanged(); } }
        }

        private string _dist;
        public string Dist
        {
            get => _dist;
            set { if (_dist != value) { _dist = value; OnPropertyChanged(); } }
        }

        private string _state;
        public string State
        {
            get => _state;
            set { if (_state != value) { _state = value; OnPropertyChanged(); } }
        }

        private string _pincode;
        public string Pincode
        {
            get => _pincode;
            set { if (_pincode != value) { _pincode = value; OnPropertyChanged(); } }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set { if (_isActive != value) { _isActive = value; OnPropertyChanged(); } }
        }
        public string Address
            => $"{House}, {Place}, {Tal}, {Dist}, {State} – {Pincode}";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
