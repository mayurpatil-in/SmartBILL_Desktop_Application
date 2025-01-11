using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBILL.Commands;
using SmartBILL.Models;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;

namespace SmartBILL.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _productKey;
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isRegistered;

        public string ProductKey
        {
            get => _productKey;
            set { _productKey = value; OnPropertyChanged(nameof(ProductKey));}
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsRegistered
        {
            get => _isRegistered;
            set { _isRegistered = value; OnPropertyChanged(nameof(IsRegistered)); }
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrWhiteSpace(ProductKey) &&
                   !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        private void Register(object parameter)
        {            
            using (var context = new RegistrationContext())
            {
                // List of valid product keys
                var validProductKeys = new List<string> { "ABC123-XYZ789", "DEF456-MNO123", "GHI789-PQR456" };

                // Check if product key is valid and not already used
                var isKeyValid = validProductKeys.Contains(ProductKey);
                var isKeyUsed = context.UsedProductKeys.Any(k => k.Key == ProductKey);

                // Check if username already exists
                var isUsernameTaken = context.Users.Any(u => u.Username == Username);

                if (isUsernameTaken)
                {
                    ErrorMessage = "* Username already exists.";
                }
                else if (isKeyValid && !isKeyUsed)
                {
                    var user = new User { Username = Username, Password = Password };
                    context.Users.Add(user);

                    // Save the product key as used
                    context.UsedProductKeys.Add(new UsedProductKey { Key = ProductKey });
                    context.SaveChanges();

                    IsRegistered = true;
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (isKeyUsed)
                {
                    ErrorMessage = "* This product key has already been used.";
                }
                else
                {
                    ErrorMessage = "* Invalid product key.";
                }
            }

        }
    }
}
