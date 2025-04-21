using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBILL.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string name = null)
        //    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // (1) single declaration of the event
        public event PropertyChangedEventHandler PropertyChanged;

        // (2) one method, optional parameter, CallerMemberName does the rest
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
