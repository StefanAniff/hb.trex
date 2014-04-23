using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    /// <summary>
    /// Interaction logic for ForecastOverviewView.xaml
    /// </summary>
    public partial class ForecastOverviewView : IForecastOverviewView
    {
        public ForecastOverviewView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            // Username sort. If done in xaml designdata will not work :/
            var viewSource = CollectionViewSource.GetDefaultView(_userRegistrationsItemsControl.Items);
            viewSource.SortDescriptions.Clear();
            viewSource.SortDescriptions.Add(new SortDescription("UserName", ListSortDirection.Ascending));
            viewSource.Refresh();
        }

        private void _searchTabControl_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            switch (_searchTabControl.SelectedIndex)
            {
                case 0:
                    Dispatcher.BeginInvoke(new Action(() => _projectAutoCompleteBox.Focus()));
                    break;
                case 1:
                    Dispatcher.BeginInvoke(new Action(() => _usrAutoCompleteBox.Focus()));
                    break;
            }
        }

        private void _btnPrint_OnClick(object sender, RoutedEventArgs e)
        {
            DoPrintResultView();
        }

        private void DoPrintResultView()
        {
            var printDlg = new PrintDialog();

            var doPrint = printDlg.ShowDialog();
            if (!doPrint.Value)
                return;

            SetButtonVisibility(Visibility.Hidden);

            var e = _resultView as FrameworkElement;

            //store original scale
            var originalScale = e.LayoutTransform;

            //get selected printer capabilities
            var capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

            //get scale of the print wrt to screen of WPF visual
            var scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / e.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                           e.ActualHeight);

            //Transform the Visual to scale
            e.LayoutTransform = new ScaleTransform(scale, scale);

            //get the size of the printer page
            var sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

            //update the layout of the visual to the printer page size.
            e.Measure(sz);
            e.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            //now print the visual to printer to fit on the one page.
            printDlg.PrintVisual(e, "Workplan");

            //apply the original transform.
            e.LayoutTransform = originalScale;
            SetButtonVisibility(Visibility.Visible);
        }

        private void SetButtonVisibility(Visibility visibility)
        {
            _previousMonthButton.Visibility = visibility;
            _currentMonthButton.Visibility = visibility;
            _calendarButton.Visibility = visibility;
            _nextMonthButton.Visibility = visibility;
        }

        private void FocusPresetNameTextBoxOnLoad(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null) return;

            textbox.Focus();
            textbox.SelectAll();
        }
    }
}
