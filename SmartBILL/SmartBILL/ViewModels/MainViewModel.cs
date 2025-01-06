using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using FontAwesome.Sharp;
using SmartBILL.Commands;


namespace SmartBILL.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentDateTime;
        private string _caption;
        private IconChar _icon;
        private ViewModelBase _currentChildView;

        #region Date and Time Dashboard
        // Property for binding with the view
        public string CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged(nameof(CurrentDateTime));
            }
        }        
        #endregion

        #region Property Main Caption and Icon
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        #endregion

        #region Property Child View
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        

        //--> Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }
        
        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomerViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }
        #endregion

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

            //Initialize commands
            ShowHomeViewCommand = new RelayCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new RelayCommand(ExecuteShowCustomerViewCommand);


            //Default view
            ExecuteShowHomeViewCommand(null);
        }
    }
}
