using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls.TimeBar;
using Telerik.Windows.Controls.Timeline;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels;

namespace Trex.SmartClient.Overview.DayOverviewScreen
{
    public partial class DayOverviewScreen : UserControl, IView
    {
        public DayOverviewScreen()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            TodayDateButton.Focus();
        }

        private void RadTimeline1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender == null) return;

            if (originalSender.DataContext.GetType() == typeof (TimelineDataItem))
            {
                var timelinedataItem = originalSender.DataContext as TimelineDataItem;
                DoubleClickTask.Command.Execute(timelinedataItem.DataItem);
            }
        }

        private void RadTimeline1_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender == null) return;

            var type = originalSender.DataContext.GetType();
            Mouse.OverrideCursor = type == typeof (TimelineDataItem)
                                       ? Cursors.Hand
                                       : null;
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

    }

    public class TrexHourFormatterProvider : IIntervalFormatterProvider
    {
        public Func<DateTime, string>[] GetFormatters(IntervalBase interval)
        {
            return new Func<DateTime, string>[]
                {
                    date => date.Hour.ToString(CultureInfo.InvariantCulture)
                };
        }

        public Func<DateTime, string>[] GetIntervalSpanFormatters(IntervalBase interval)
        {
            return new Func<DateTime, string>[]
                {
                    date => String.Format("{0} ... {1}", date.Hour, interval.IncrementByCurrentInterval(date).Hour)
                };
        }
    }
}