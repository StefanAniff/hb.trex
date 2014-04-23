using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.FieldList;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Forecast.Shared.ViewHelpers;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    /// <summary>
    /// Interaction logic for ForecastRegistrationView.xaml
    /// </summary>
    public partial class ForecastRegistrationView : UserControl, IForecastRegistrationView
    {
        public ForecastRegistrationView()
        {
            InitializeComponent();            
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }        

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);            

            // Sort clients by name
            var viewSource = CollectionViewSource.GetDefaultView(_clientsItemsControl.Items);
            viewSource.SortDescriptions.Clear();
            viewSource.SortDescriptions.Add(new SortDescription("ProjectName", ListSortDirection.Ascending));
            viewSource.Refresh();
        }

        #region Client search focus/navigation handling

        private void _tbxClientSearch_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Down key leave focus
            if (e.Key == Key.Down && _searchResultPopup.IsOpen)
            {
                var tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                _tbxClientSearch.MoveFocus(tRequest);
                e.Handled = true;
            }
        }

        private void _tbxClientSearch_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!_searchResultPopup.IsOpen)
                return;

            // Force focus on first element in listbox
            _searchResultBox.Focus();
            var listBxItem = (ListBoxItem) _searchResultBox.ItemContainerGenerator.ContainerFromIndex(0);
            FocusManager.SetFocusedElement(_searchResultBox, listBxItem);            
        }

        private void _searchResultPopup_OnKeyDown(object sender, KeyEventArgs e)
        {
            // Shift-Tab or Up to focus back on _tbxClientSearch
            if ((e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Tab) || e.Key == Key.Up || e.Key == Key.Escape)
            {
                //KeyboardNavigation.
                FocusClientSearchTextBox();
                e.Handled = true;
            }
        }

        private void FocusClientSearchTextBox()
        {
            FocusManager.SetFocusedElement(_tbxClientSearch.Parent, _tbxClientSearch);
            _tbxClientSearch.Focus();
        }        

        #endregion

        private void grdClientReg_ClientLineLoaded(object sender, RoutedEventArgs e)
        {
            // Grids are disabled. Don't focus stuff
            if (!_grdProjectRegGroup.IsEnabled)
                return;

            // When client line is loaded give first hour-registration textbox focus
            FocusHelper.DoFocusNext(sender);
            var textBox = Keyboard.FocusedElement as TextBox;

            var loopGuard = new EndlessLoopFocusGuard();
            while (textBox != null && textBox.Name != "_tbxClientHours")
            {
                if (loopGuard.Visited(textBox))
                {
                    /** If this is shown, there is an error when trying to focus _tbxClientHours. 
                     * This is easier to debug, than an endless loop while loading data.
                     */
                    MessageBox.Show("Something broke the search for _tbxClientHours focus");
                    break;
                }

                FocusHelper.DoFocusNext(textBox);
                textBox = Keyboard.FocusedElement as TextBox;                                
            }
        }

        private class EndlessLoopFocusGuard
        {
            private readonly ISet<FrameworkElement> _visited = new HashSet<FrameworkElement>();

            public bool Visited(FrameworkElement element)
            {
                if (_visited.Contains(element))
                    return true;

                _visited.Add(element);
                return false;
            }
        }

        private void PresenceContentControlLoaded(object sender, RoutedEventArgs e)
        {
            FocusClientSearchTextBox();
        }

        private void _searchResultBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null)
                return;

            var keyBinding =
                listBox.InputBindings.Cast<object>()
                       .OfType<KeyBinding>().FirstOrDefault(keybinding => keybinding.Key == Key.Enter);

            if (keyBinding == null)
                return;

            keyBinding.Command.Execute(null);
        }
    }        
}
