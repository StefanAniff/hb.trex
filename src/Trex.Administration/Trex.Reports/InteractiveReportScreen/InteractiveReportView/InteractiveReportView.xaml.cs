using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Commands;

namespace Trex.Reports.InteractiveReportScreen.InteractiveReportView
{
    public partial class InteractiveReport : IView
    {
        public InteractiveReport()
        {
            InitializeComponent();
            Loaded += InteractiveReport_Loaded;
            GridView.Grouped += GridView_Grouped;
            GridView.Filtered += GridView_Filtered;
            GridView.DataLoaded += GridView_DataLoaded;
            GridView.Grouping += GridView_Grouping;
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion

        private void InteractiveReport_Loaded(object sender, RoutedEventArgs e)
        {
            //Chart.AnimationSettings.ItemDelay = TimeSpan.FromMilliseconds(0);
            ////Chart.AnimationSettings.ItemAnimationDuration = TimeSpan.FromMilliseconds(500);
            //Chart.AnimationSettings.DefaultSeriesDelay = TimeSpan.FromMilliseconds(0);
            //Chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 75;
            //Chart.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "N2";

            //Chart.DefaultView.ChartTitle.BorderThickness = new Thickness(0);

            //Chart.DefaultView.ChartLegend.Header = ChartStrings.ChartChartLegendHeader;
            //Chart.DefaultView.ChartArea.NoDataString = ChartStrings.ChartNoDataMessage;
            //Chart.PaletteBrushes.Clear();
            //Chart.PaletteBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 189, 255)));
        }

        private void GridView_Filtered(object sender, GridViewFilteredEventArgs e)
        {
            BindChart();
        }

        private void GridView_Grouping(object sender, GridViewGroupingEventArgs e)
        {
            ApplicationCommands.SystemBusy.Execute("Grupperer data");
        }

        private void GridView_Grouped(object sender, GridViewGroupedEventArgs e)
        {
            AddAggregateDescriptions();

            BindChart();
            ApplicationCommands.SystemIdle.Execute(null);
        }

        private void AddAggregateDescriptions()
        {
            if (GridView.GroupDescriptors.Count > 0)
            {
                if (((ColumnGroupDescriptor) (GridView.GroupDescriptors[0])).Column.AggregateFunctions.Count == 0)
                {
                    ((ColumnGroupDescriptor) (GridView.GroupDescriptors[0])).Column.AggregateFunctions.Add(new SumFunction
                                                                                                               {SourceField = "TimeSpent", Caption = "Total time spent:", ResultFormatString = "{0:N2}"});
                }
            }
        }

        private void GridView_DataLoaded(object sender, EventArgs e)
        {
            BindChart();
        }

        private void BindChart()
        {
            //Chart.ItemsSource = null;
            //Chart.SeriesMappings.Clear();

            //var mapping = GetSelectedMapping();
            //var label = string.Empty;

            //if (GridView.GroupDescriptors.Count > 0)
            //{
            //    var count = 0;
            //    foreach (ColumnGroupDescriptor descriptor in GridView.GroupDescriptors)
            //    {
            //        mapping.GroupingSettings.GroupDescriptors.Add(new ChartGroupDescriptor(descriptor.Column.UniqueName));

            //        count++;
            //    }

            //    var lastGroupDescriptor = (ColumnGroupDescriptor) GridView.GroupDescriptors[GridView.GroupDescriptors.Count - 1];
            //    mapping.ItemMappings.Add(new ItemMapping(lastGroupDescriptor.Column.UniqueName, DataPointMember.XCategory));
            //}

            ////Chart.DefaultView.ChartLegend.Header = LEGEND_NAME;
            //Chart.SeriesMappings.Add(mapping);
            //var chartSource = GetChartSource();
            //try
            //{
            //    if (chartSource != null)
            //    {
            //        Chart.ItemsSource = chartSource;
            //    }
            //}
            //catch (NullReferenceException)
            //{
            //    //Swallow null reference, because of bug in telerik chart
            //}

            ////_firstViewChartLoaded = true;
        }

        //private SeriesMapping GetSelectedMapping()
        //{
        //    var selectedMeasure = "TimeSpent";

        //    var mapping1 = new SeriesMapping();

        //    var series1 = new BarSeriesDefinition();
        //    series1.Appearance.Foreground = new SolidColorBrush(Colors.Black);
        //    series1.Appearance.Stroke = new SolidColorBrush(Colors.White);

        //    mapping1.ItemMappings.Add(new ItemMapping(selectedMeasure, DataPointMember.YValue, ChartAggregateFunction.Sum));

        //    series1.ItemLabelFormat = "N0";
        //    mapping1.SeriesDefinition = series1;

        //    mapping1.SeriesDefinition.ShowItemLabels = true;
        //    return mapping1;
        //}

        private object GetChartSource()
        {
            var chartSource = new List<TimeEntryViewModel>();

            foreach (var item in GridView.Items)
            {
                if (item is AggregateFunctionsGroup)
                {
                    GetGroupItems(item as AggregateFunctionsGroup, chartSource);
                }
                else
                {
                    var f = item as TimeEntryViewModel;

                    chartSource.Add(f);
                }
            }
            if (chartSource.Count == 0)
            {
                return GridView.ItemsSource;
            }

            return chartSource;
        }

        private void GetGroupItems(AggregateFunctionsGroup group, List<TimeEntryViewModel> sourceList)
        {
            if (group.HasSubgroups)
            {
                foreach (var subGroup in group.Items)
                {
                    GetGroupItems(subGroup as AggregateFunctionsGroup, sourceList);
                }
            }

            else
            {
                var tmpTable = new List<TimeEntryViewModel>();
                foreach (var item in group.Items)
                {
                    tmpTable.Add(item as TimeEntryViewModel);
                }
                sourceList.AddRange(tmpTable);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //_parentControl = this.Parent as ContentControl;
            //_parentControl.Content = null;
            if (!Application.Current.Host.Content.IsFullScreen)
            {
                Application.Current.Host.Content.IsFullScreen = true;
                ApplicationCommands.GotoFullScreenMode.Execute(this);
            }
        }
    }
}