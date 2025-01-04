using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SmartBILL.ViewModels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        private string _currentDateTime;

        // Property for binding with the view
        public string CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // Initialize and start the timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Update every second
            timer.Tick += (sender, args) =>
            {
                CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, hh:mm tt"); // "F" for full date/time pattern dddd, MMMM d, yyyy, hh:mm tt
            };
            timer.Start();
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
