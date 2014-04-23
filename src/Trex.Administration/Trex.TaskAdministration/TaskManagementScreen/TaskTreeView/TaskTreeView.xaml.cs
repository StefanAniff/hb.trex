#region

using System;
using System.Collections;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.DragDrop;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Interfaces;

#endregion

namespace Trex.TaskAdministration.TaskManagementScreen.TaskTreeView
{
    public partial class TaskTreeView : UserControl, ITaskTreeView
    {
        public TaskTreeView()
        {
            InitializeComponent();
            //taskTree.AddHandler(RadDragAndDropManager.DropQueryEvent, new EventHandler<DragDropQueryEventArgs>(TreeDropQuery), true);

            //taskTree.AddHandler(RadDragAndDropManager.DropInfoEvent, new EventHandler<DragDropEventArgs>(TreeDropInfo), true);

            //taskTree.PreviewDragEnded += taskTree_PreviewDragEnded;

            //taskTree.DropExpandDelay = TimeSpan.FromSeconds(1);
        }

        #region ITaskTreeView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        //private void TreeDropInfo(object sender, DragDropEventArgs e)
        //{
        //    var destination = e.Options.Destination.DataContext as IDraggable;
        //    IDraggable draggedObject = null;

        //    if (e.Options.Payload is IEnumerable)
        //    {
        //        var enumerator = ((IEnumerable)e.Options.Payload).GetEnumerator();
        //        if (enumerator.MoveNext())
        //        {
        //            draggedObject = enumerator.Current as IDraggable;
        //        }
        //    }
        //    else if (e.Options.Payload is IDraggable)
        //    {
        //        draggedObject = e.Options.Payload as IDraggable;
        //    }

        //    if (e.Options.Status == DragStatus.DropPossible)
        //    {
        //    }

        //    if (e.Options.Status == DragStatus.DropComplete)
        //    {
        //        e.Handled = true;
        //        destination.RecieveDraggable(draggedObject);
        //    }
        //}

        //private void TreeDropQuery(object sender, DragDropQueryEventArgs e)
        //{
        //    IDraggable draggedItem = null;
        //    var destinationTreeItem = e.Options.Destination as RadTreeViewItem;

        //    if (destinationTreeItem != null && destinationTreeItem.DropPosition != DropPosition.Inside)
        //    {
        //        e.QueryResult = false;
        //        e.Handled = true;
        //        return;
        //    }

        //    if (e.Options.Payload is IEnumerable)
        //    {
        //        var enumerator = ((IEnumerable)e.Options.Payload).GetEnumerator();
        //        if (enumerator.MoveNext())
        //        {
        //            draggedItem = enumerator.Current as IDraggable;
        //        }
        //    }
        //    else if (e.Options.Payload is IDraggable)
        //    {
        //        draggedItem = e.Options.Payload as IDraggable;
        //    }

        //    var destinationItem = e.Options.Destination.DataContext as IDraggable;

        //    if (destinationItem != null && draggedItem != null)
        //    {
        //        e.QueryResult = destinationItem.Entity.IsValidChild(draggedItem.Entity);
        //    }
        //    else
        //    {
        //        e.QueryResult = false;
        //    }

        //    e.Handled = true;
        //}

        //private void taskTree_PreviewDragEnded(object sender, RadTreeViewDragEndedEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //private void RadTreeView_LoadOnDemand(object sender, RadRoutedEventArgs e)
        //{
        //    var treeViewItem = e.OriginalSource as RadTreeViewItem;

        //    var treeViewItemViewModel = treeViewItem.Item as TreeViewItemViewModel;
        //    //treeViewItem.ItemsSource = treeViewItemViewModel.Children;

        //    treeViewItemViewModel.IsExpanded = true;
        //    //treeViewItemViewModel.IsLoadOnDemandEnabled = false;
        //}
    }
}