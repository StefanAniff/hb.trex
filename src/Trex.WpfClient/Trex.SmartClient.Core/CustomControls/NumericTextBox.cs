using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Trex.SmartClient.Core.CustomControls
{
    /// <summary>
    /// Only numeric input allowed.
    /// Some shortcut commands are also accepted.
    /// </summary>
    public class NumericTextBox : TextBox, IDisposable
    {
        private bool _updateSourceOnOpeningContextMenu;
        private bool _selectAllTextOnFocus;
        private readonly NumericKeyValidator _validator = new NumericKeyValidator();

        /// <summary>
        /// Indicates if source should be updated, when contextmenu 
        /// is opening
        /// </summary>
        public bool UpdateSourceOnOpeningContextMenu
        {
            get { return _updateSourceOnOpeningContextMenu; }
            set
            {
                _updateSourceOnOpeningContextMenu = value;
                if (_updateSourceOnOpeningContextMenu)
                    ContextMenuOpening += OnUserContextMenuOpening;
                else
                    ContextMenuOpening -= OnUserContextMenuOpening;
            }
        }

        /// <summary>
        /// Indicates if the 
        /// </summary>
        public bool SelectAllTextOnFocus
        {
            get { return _selectAllTextOnFocus; }
            set
            {
                _selectAllTextOnFocus = value;
                if (_selectAllTextOnFocus)
                {
                    GotKeyboardFocus += OnUserGotKeyboardFocus;
                    GotMouseCapture += OnUserGotMouseCapture;   
                }
                else
                {
                    GotKeyboardFocus -= OnUserGotKeyboardFocus;
                    GotMouseCapture -= OnUserGotMouseCapture;
                }
            }
        }

        public NumericTextBox()
        {
            KeyDown += OnUserInput;
        }

        private void OnUserGotMouseCapture(object sender, MouseEventArgs e)
        {
            TextBoxSelectAll(sender);
        }

        private void OnUserGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBoxSelectAll(sender);            
        }

        private static void TextBoxSelectAll(object sender)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.SelectAll();
        }

        public void OnUserInput(object sender, KeyEventArgs e)
        {
            e.Handled = !_validator.IsValid(e.Key, e.SystemKey, Keyboard.Modifiers);
        }

        public virtual ModifierKeys CurrentKeyModifiers
        {
            get { return Keyboard.Modifiers; }
        }

        private void OnUserContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TryUpdateTextBoxBindingSource(sender);
        }

        private void TryUpdateTextBoxBindingSource(object sender)
        {
            var tbx = sender as TextBox;

            if (tbx == null || !tbx.Equals(this))
                return;

            var binding = BindingOperations.GetBindingExpression(tbx, TextProperty);
            if (binding == null)
                return;

            binding.UpdateSource();
        }

        public void Dispose()
        {
            // Cleanup subscriptions
            KeyDown -= OnUserInput;

            if (UpdateSourceOnOpeningContextMenu)
            {
                ContextMenuOpening -= OnUserContextMenuOpening;
            }

            if (SelectAllTextOnFocus)
            {
                GotKeyboardFocus -= OnUserGotKeyboardFocus;
                GotMouseCapture -= OnUserGotMouseCapture;
            }
        }

        public class NumericKeyValidator
        {
            public bool IsValid(Key key, Key systemKey, ModifierKeys currentKeyModifiers)
            {
                var isValid = true;

                // Shift may only be used in combination with tab or F10 (contextmenu/right-click-menu)
                if (currentKeyModifiers == ModifierKeys.Shift && key == Key.System && systemKey == Key.F10)
                {
                    return true;
                }

                //All control and alt modifiers valid except ALT Gr(CTRL+ALT)+1-9
                if (((currentKeyModifiers & ModifierKeys.Control) == (ModifierKeys.Control)) && ((currentKeyModifiers & ModifierKeys.Alt) == (ModifierKeys.Alt)))
                {
                    isValid = false;
                }
                else
                {
                    if (((currentKeyModifiers & ModifierKeys.Alt) == ModifierKeys.Alt) ||
                        ((currentKeyModifiers & ModifierKeys.Control) == ModifierKeys.Control))
                    {
                        return true;
                    }
                }

                if ((currentKeyModifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    isValid &= (key == Key.Tab || key == Key.D5);
                }

                isValid &= ((key >= Key.D0 && key <= Key.D9) ||
                            (key >= Key.NumPad0 && key <= Key.NumPad9) ||
                            key == Key.OemComma || key == Key.OemPeriod ||
                            (key >= Key.F1 && key <= Key.F12) || key == Key.Escape ||
                            key == Key.Enter || key == Key.Tab || key == Key.Decimal);

                return isValid;
            }
        }
    }
}