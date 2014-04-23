using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.TreeListView;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public partial class MainListView : UserControl, IView
    {
        public MainListView()
        {
            // Required to initialize variables
            InitializeComponent();
            //using Telerik.Windows;
            ListView.AddHandler(RadDragAndDropManager.DropQueryEvent, new EventHandler<DragDropQueryEventArgs>(TreeDropQuery), true);

            ListView.AddHandler(RadDragAndDropManager.DropInfoEvent, new EventHandler<DragDropEventArgs>(TreeDropInfo), true);

            ListView.PreviewDragEnded += PreviewDragEnded;
            InternalCommands.ExcelExportStart.RegisterCommand(new DelegateCommand(ExcelExportExecute));
            //InternalCommands.ItemSelected.RegisterCommand(new DelegateCommand<ListItemModelBase>(ItemSelected));

            //ListView.RowLoaded += new EventHandler<Telerik.Windows.Controls.GridView.RowLoadedEventArgs>(ListView_RowLoaded);
            //ListView.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(ListView_SelectionChanged);
            //ListView.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
            //ListView.RowIsExpandedChanged += new EventHandler<RowEventArgs>(ListView_RowIsExpandedChanged);
            //ListView.DataLoaded += new EventHandler<System.EventArgs>(ListView_DataLoaded);

            //ListView.Export(new MemoryStream(),new GridViewExportOptions() {Format = ExportFormat.Csv});
        }

        void ItemContainerGenerator_StatusChanged(object sender, System.EventArgs e)
        {
            if (ListView.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                foreach (var listItem in ListView.Items)
                {
                    var row = ListView.ItemContainerGenerator.ContainerFromItem(listItem) as TreeListViewRow;

                    if (row != null)
                    {
                        row.IsExpanded = true;

                        //row.SetBinding(TreeListViewRow.IsSelectedProperty, new Binding("IsSelected"));
                    }

                }
            }
        }

        private void ItemSelected(ListItemModelBase obj)
        {

            var row = ListView.ItemContainerGenerator.ContainerFromItem(obj) as TreeListViewRow;
            //ListView.Items.AsQueryable().Where<>(i => i.Entity.Id == obj.Entity.Id);

            if (row != null)
            {
                row.IsSelected = true;
                //row.Focus();
            }
        }

        void ListView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {


        }

        void ListView_RowIsExpandedChanged(object sender, RowEventArgs e)
        {
            //  var row = e.Row;
        }

        void ListView_DataLoaded(object sender, System.EventArgs e)
        {
            foreach (var listItem in ListView.Items)
            {
                var row = ListView.ItemContainerGenerator.ContainerFromItem(listItem) as TreeListViewRow;

                if (row != null)
                {
                    row.IsExpanded = true;
                    row.IsSelected = true;
                    //row.SetBinding(TreeListViewRow.IsSelectedProperty, new Binding("IsSelected"));
                }

            }
        }

        void ListView_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            var listItem = e.DataElement as ListItemModelBase;

            var row = ListView.ItemContainerGenerator.ContainerFromItem(listItem) as GridViewRow;
            if (row != null)
            {
                var status = ListView.ItemContainerGenerator.Status;

                // row.SetBinding(GridViewRow.IsExpandedProperty, new Binding(){Source = listItem,Path = new PropertyPath("IsExpanded"),Mode = BindingMode.TwoWay});
                //if (listItem.IsSelected)
                // row.IsSelected = listItem.IsSelected;
                // row.IsExpanded = false;
                //row.IsExpanded = true;

                ListView.ExpandHierarchyItem(listItem);

                //ListView.ExpandHierarchyItem(listItem);
                // row.SetBinding(TreeListViewRow.IsSelectedProperty, new Binding("IsSelected"));
            }


        }

        //    //if (listItem != null)
        //    //    if (listItem.IsExpanded)
        //    //        ListView.ExpandHierarchyItem(e.Row);

        //    //ListView.ExpandAllHierarchyItems();

        //}

        void ExpandTree(GridViewRowItem row)
        {

            ListView.ExpandHierarchyItem(row);
            var parent = row.Parent as GridViewRowItem;
            if (parent != null)
                ExpandTree(parent);
        }

        private void ExcelExportExecute(object obj)
        {
            const string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {


                using (Stream stream = dialog.OpenFile())
                {

                    ListView.Export(stream,
                                    new GridViewExportOptions()
                                        {
                                            Format = ExportFormat.ExcelML,
                                            ShowColumnHeaders = true,
                                            ShowColumnFooters = true,
                                            ShowGroupFooters = false,
                                            Items = ListView.Items



                                        });
                }
            }
        }

        private void PreviewDragEnded(object sender, RadTreeListViewDragEndedEventArgs e)
        {
            e.Handled = true;
        }

        private void TreeDropInfo(object sender, DragDropEventArgs e)
        {

            var destination = e.Options.Destination.DataContext as IDraggable;
            var draggedObjects = new List<IDraggable>();

            if (e.Options.Payload is IEnumerable)
            {
                var enumerator = ((IEnumerable)e.Options.Payload).GetEnumerator();
                draggedObjects.AddRange(from object trigger in e.Options.Payload as IEnumerable where enumerator.MoveNext() select enumerator.Current as IDraggable);
            }
            else if (e.Options.Payload is IDraggable)
            {
                draggedObjects.Add(e.Options.Payload as IDraggable);
            }

            if (e.Options.Status == DragStatus.DropPossible) { }

            if (e.Options.Status == DragStatus.DropComplete && destination != null)
            {
                e.Handled = true;
                foreach (var draggable in draggedObjects)
                {
                    destination.RecieveDraggable(draggable);
                }
            }
        }

        private void TreeDropQuery(object sender, DragDropQueryEventArgs e)
        {
            var draggedItems = new List<IDraggable>();
            var items = ListView.SelectedItems as IEnumerable;
            
            var t = e.Options.Source.DataContext as ListTimeEntryViewModel;
            if (t != null && t.TimeEntry.DocumentType == 2)
            {
                e.Handled = false;
                return;
            }
            if (e.Options.Source.DataContext is ListTaskViewModel || e.Options.Source.DataContext is ListProjectViewModel || e.Options.Source.DataContext is ListCustomerViewModel)
            {
                e.Handled = false;
                return;
            }

            var destinationTreeItem = e.Options.Destination as RadTreeViewItem;

            if (destinationTreeItem != null && destinationTreeItem.DropPosition != DropPosition.Inside)
            {
                e.QueryResult = false;
                e.Handled = true;
                return;
            }

            if (e.Options.Payload is IEnumerable)
            {
                var enumerator = ((IEnumerable)e.Options.Payload).GetEnumerator();

                draggedItems.AddRange(from object item in items where enumerator.MoveNext() select enumerator.Current as IDraggable);
            }
            else if (e.Options.Payload is IDraggable)
            {
                draggedItems.Add(e.Options.Payload as IDraggable);
            }

            var destinationItem = e.Options.Destination.DataContext as IDraggable;

            if (destinationItem != null && draggedItems != null)
            {
                foreach (var draggedIte in draggedItems)
                {
                    e.QueryResult = destinationItem.Entity.IsValidChild(draggedIte.Entity);
                }
            }
            else
            {
                e.QueryResult = false;
            }

            e.Handled = true;
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}