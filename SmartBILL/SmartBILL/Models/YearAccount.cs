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
    public class YearAccount : INotifyPropertyChanged
    {
        private int _yearId;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isActive;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int YearId
        {
            get => _yearId;
            set => SetField(ref _yearId, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetField(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetField(ref _endDate, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetField(ref _isActive, value);
        }
        public virtual ICollection<PartyChallan> PartyChallans { get; set; } = new HashSet<PartyChallan>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }
    }
}
