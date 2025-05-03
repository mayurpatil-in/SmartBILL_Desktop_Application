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
    public class ProcessItem : INotifyPropertyChanged
    {
        private int _processId;
        [Key]
        public int ProcessId
        {
            get => _processId;
            set => SetProperty(ref _processId, value);
        }

        private string _processName;
        [MaxLength(100)]
        public string ProcessName
        {
            get => _processName;
            set => SetProperty(ref _processName, value);
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
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
