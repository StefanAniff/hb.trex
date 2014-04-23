using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Reporting.ReportScreen.ReportScreenMasterView
{
    public partial class ReportScreenMasterview : UserControl, IView
    {
        public ReportScreenMasterview()
        {
            InitializeComponent();
       
            var mapping = Chart.SeriesMappings.First();
            try
            {
                var itemMapping = new ItemMapping("TimeSpent", DataPointMember.YValue, ChartAggregateFunction.Sum);
                mapping.ItemMappings.Add(itemMapping);
            }
            catch (Exception)
            {
                
            }
         

            GridView.Grouped += (sender, args) => BindChart();
            GridView.Filtered += (sender, args) => BindChart();
            GridView.DataLoaded += GridViewOnDataLoaded;
            BtnClearFilter.Click += btnClearFilter_Clicked;
            ReportCommands.DataLoaded.RegisterCommand(new DelegateCommand<object>(o => BindChart()));
        }

        private void GridViewOnDataLoaded(object sender, EventArgs eventArgs)
        {
            var chartSource = GetChartSource();
            var mapping = Chart.SeriesMappings.First();

            if (chartSource != null)
            {
                mapping.ItemsSource = chartSource;
            }

            if (GridView.Items.Groups != null && GridView.Items.Groups.Count > 2)
            {
                var groupKeys = GridView.Items.Groups
                                        .OfType<QueryableCollectionViewGroup>()
                                        .Select(x => x.Key)
                                        .ToList();
                //if (groupKeys.All(x => x.ToString().Length < 12))
                //{
                //    Chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 90;
                //}
                if (!groupKeys.Any(x => x.ToString().Length > 35))
                {
                    Chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 45;
                }
                else
                {
                    Chart.DefaultView.ChartArea.AxisX.StepLabelLevelCount = 3;
                    Chart.DefaultView.ChartArea.AxisX.StepLabelLevelHeight = 10;
                    Chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 0;
                }
            }
            else
            {
                Chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 0;
            }
        }

        private void btnClearFilter_Clicked(object sender, RoutedEventArgs e)
        {
            GridView.FilterDescriptors.Clear();
        }

        private void BindChart()
        {
            Chart.ItemsSource = null;

            var mapping = Chart.SeriesMappings.First();
            mapping.GroupingSettings.GroupDescriptors.Clear();
            if (GridView.GroupDescriptors.Count > 0)
            {
                mapping.LegendLabel = null;
                var memberName = string.Empty;

                foreach (var descriptor in GridView.GroupDescriptors.OfType<ColumnGroupDescriptor>())
                {
                    var gridViewColumn = descriptor.Column as GridViewDataColumn;
                    memberName = gridViewColumn.UniqueName;
                    var memberType = gridViewColumn.DataType;

                    if (memberName == "Date")
                    {
                        memberName = "ChartDate";
                        memberType = typeof (string);
                    }
                    var chartGroupDescriptor = new ChartGroupDescriptor(memberName);
                    chartGroupDescriptor.MemberType = memberType;
                    mapping.GroupingSettings.GroupDescriptors.Add(chartGroupDescriptor);
                }

                var itemMapping = new ItemMapping(memberName, DataPointMember.XCategory);
                mapping.ItemMappings.Add(itemMapping);
            }
            else
            {
                mapping.LegendLabel = "Timespent";
                Chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 0;
            }

            var chartSource = GetChartSource();
            if (chartSource != null)
            {
                mapping.ItemsSource = chartSource;
            }
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private object GetChartSource()
        {
            var chartSource = new List<object>();

            foreach (var item in GridView.Items)
            {
                if (item is IGroup)
                {
                    chartSource.AddRange(GetGroupItems(item as IGroup));
                }
                else
                {
                    chartSource.Add(item);
                }
            }
            return chartSource.Count == 0 ? GridView.ItemsSource : chartSource;
        }


        private static IEnumerable<object> GetGroupItems(IGroup columnGroup)
        {
            var sourceList = new List<object>();
            if (!columnGroup.HasSubgroups)
            {
                return columnGroup.Items.OfType<object>();
            }
            foreach (var subGroup in columnGroup.Items.OfType<AggregateFunctionsGroup>())
            {
                sourceList.AddRange(GetGroupItems(subGroup));
            }
            return sourceList;
        }

        private void RadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SaveButton.IsEnabled)
            {
                EditTaskButton.Command.Execute(null);
            }
        }
    }
}
