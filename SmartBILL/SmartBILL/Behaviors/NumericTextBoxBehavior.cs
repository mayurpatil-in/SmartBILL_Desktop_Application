using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace SmartBILL.Behaviors
{
    public static class NumericTextBoxBehavior
    {
        // Allow digits with an optional single decimal point
        private static readonly Regex _numericRegex =
            new Regex(@"^\d*\.?\d*$", RegexOptions.Compiled);

        public static readonly DependencyProperty AllowOnlyNumericProperty =
            DependencyProperty.RegisterAttached(
                "AllowOnlyNumeric",
                typeof(bool),
                typeof(NumericTextBoxBehavior),
                new UIPropertyMetadata(false, OnAllowOnlyNumericChanged));

        public static bool GetAllowOnlyNumeric(DependencyObject obj) =>
            (bool)obj.GetValue(AllowOnlyNumericProperty);

        public static void SetAllowOnlyNumeric(DependencyObject obj, bool value) =>
            obj.SetValue(AllowOnlyNumericProperty, value);

        private static void OnAllowOnlyNumericChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                if ((bool)e.NewValue)
                {
                    tb.PreviewTextInput += TextBox_PreviewTextInput;
                    DataObject.AddPastingHandler(tb, TextBox_OnPaste);
                }
                else
                {
                    tb.PreviewTextInput -= TextBox_PreviewTextInput;
                    DataObject.RemovePastingHandler(tb, TextBox_OnPaste);
                }
            }
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            // Simulate the text as if input goes through
            string fullText = tb.Text.Insert(tb.CaretIndex, e.Text);
            if (!_numericRegex.IsMatch(fullText))
            {
                e.Handled = true;
                MessageBox.Show("Please enter a valid number (digits and at most one decimal point).",
                                "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private static void TextBox_OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var pasted = e.DataObject.GetData(DataFormats.Text) as string;
                var tb = (TextBox)sender;
                // Simulate resulting text after paste
                string fullText = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength)
                                 .Insert(tb.SelectionStart, pasted);

                if (!_numericRegex.IsMatch(fullText))
                {
                    e.CancelCommand();
                    MessageBox.Show("Please paste a valid number (digits and at most one decimal point).",
                                    "Invalid Paste", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
