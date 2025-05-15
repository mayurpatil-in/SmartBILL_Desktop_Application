using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using FontAwesome.Sharp;
using SmartBILL.Commands;
using SmartBILL.Models;
using SmartBILL.Views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace SmartBILL.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentDateTime;
        private string _caption;
        private IconChar _icon;
        private ViewModelBase _currentChildView;
        public string WelcomeMessage { get; }

        private bool _isCustomersExpanded;
        public bool IsCustomersExpanded
        {
            get => _isCustomersExpanded;
            set
            {
                _isCustomersExpanded = value;
                OnPropertyChanged(nameof(IsCustomersExpanded));
            }
        }
        private bool _isPartyExpanded;
        public bool IsPartyExpanded
        {
            get => _isPartyExpanded;
            set
            {
                _isPartyExpanded = value;
                OnPropertyChanged(nameof(IsPartyExpanded));
            }
        }

        #region Copyrights Year
        // Fixed start year
        private const int BaseYear = 2025;

        // Exposed property for binding
        public string YearRange
            => $"{BaseYear}-{DateTime.Now.Year + 1}";

        // (If you want © and a comma:)
        public string CopyrightNotice
            => $"© {YearRange}, ";

        #endregion

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
        public ICommand ShowCustomerPartyViewCommand { get; }
        public ICommand ShowItemViewCommand { get; }
        public ICommand ShowPartyChallanViewCommand { get; }
        public ICommand ShowPartyChallanSearchViewCommand { get; }
        public ICommand ShowDeliveryChallanViewCommand { get; }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomerUserViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
            
        }
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
            
        }
        private void ExecuteShowCustomerPartyViewCommand(object obj)
        {
            CurrentChildView = new CustomerPartyViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;

        }
        private void ExecuteShowItemViewCommand(object obj)
        {
            CurrentChildView = new ItemViewModel();
            Caption = "Items List";
            Icon = IconChar.List;

        }
        private void ExecuteShowPartyChallanViewCommand(object obj)
        {
            CurrentChildView = new PartyChallanViewModel();
            Caption = "Party Challan";
            Icon = IconChar.Industry;
            
        }
        private void ExecuteShowPartyChallanSearchViewCommand(object obj)
        {
            CurrentChildView = new PartyChallanSearchViewModel(this);
            Caption = "Party Challan";
            Icon = IconChar.Industry;

        }
        private void ExecuteShowDeliveryChallanViewCommand(object obj)
        {
            CurrentChildView = new DeliveryChallanViewModel();
            Caption = "Delivery Challan";
            Icon = IconChar.TruckFast;

        }
        #endregion

        public MainViewModel(string username)
        {
            WelcomeMessage = $"Welcome, {username}!";
            // Initialize and start the timer
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Update every second
            };
            timer.Tick += (sender, args) =>
            {
                CurrentDateTime = DateTime.Now.ToString("dd MMMM yyyy, hh:mm tt"); // "F" for full date/time pattern dddd, MMMM d, yyyy, hh:mm tt
            };
            timer.Start();



            //Initialize commands
            ShowHomeViewCommand = new RelayCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new RelayCommand(ExecuteShowCustomerViewCommand);
            ShowCustomerPartyViewCommand = new RelayCommand(ExecuteShowCustomerPartyViewCommand);
            ShowItemViewCommand = new RelayCommand(ExecuteShowItemViewCommand);
            ShowPartyChallanViewCommand = new RelayCommand(ExecuteShowPartyChallanViewCommand);
            ShowPartyChallanSearchViewCommand = new RelayCommand(ExecuteShowPartyChallanSearchViewCommand);
            ShowDeliveryChallanViewCommand = new RelayCommand(ExecuteShowDeliveryChallanViewCommand);
            //Default view
            ExecuteShowHomeViewCommand(null);
        }
    }
}
