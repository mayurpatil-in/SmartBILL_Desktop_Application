using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SmartBILL.CustomControl
{
    public static class PasswordBoxHelper
    {
        // Register attached property
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnPasswordChanged));

        // Set the password value
        public static void SetPassword(DependencyObject element, string value)
        {
            element.SetValue(PasswordProperty, value);
        }

        // Get the password value
        public static string GetPassword(DependencyObject element)
        {
            return (string)element.GetValue(PasswordProperty);
        }

        // When the password changes, update the PasswordBox
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox != null && passwordBox.Password != (string)e.NewValue)
            {
                passwordBox.Password = (string)e.NewValue;
            }
        }

        // Handle PasswordChanged event to update the attached property
        public static void OnPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                SetPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}
