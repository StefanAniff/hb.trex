using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.TreeView;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.Controls
{
    public class DragAndDropListBox : ListBox
    {
        public DragAndDropListBox()
        {
            this.AddHandler(RadDragAndDropManager.DragQueryEvent, new EventHandler<DragDropQueryEventArgs>(OnDragQuery));
            this.AddHandler(RadDragAndDropManager.DropQueryEvent, new EventHandler<DragDropQueryEventArgs>(OnDropQuery));
            this.AddHandler(RadDragAndDropManager.DropInfoEvent, new EventHandler<DragDropEventArgs>(OnDropInfo));
            this.AddHandler(RadDragAndDropManager.DragInfoEvent, new EventHandler<DragDropEventArgs>(OnDragInfo));

            RadDragAndDropManager.SetAllowDrop(this, true);
        }

        private void OnDragInfo(object sender, DragDropEventArgs e)
        {
            if (e.Options.Status == DragStatus.DragInProgress)
            {
                var draggedItem = e.Options.Payload as IDraggable;
                var cue = new TreeViewDragCue();
                cue.ItemTemplate = ItemTemplate;
                var itemsSource = new List<IDraggable>();
                itemsSource.Add(draggedItem);
                cue.ItemsSource = itemsSource;
                e.Options.DragCue = cue;
            }
        }

        private void OnDropInfo(object sender, DragDropEventArgs e)
        {
            //ICollection draggedItems = e.Options.Payload as ICollection<>;
            //var draggedItem = e.Options.Payload as IDraggable;
            var cue = e.Options.DragCue as TreeViewDragCue;
            //cue.ItemTemplate = this.ItemTemplate;
            //if (draggedItem != null)
            //    cue.DragActionContent = draggedItem.Entity.ToString();
            if (e.Options.Status == DragStatus.DropPossible)
            {
                //cue.DragActionContent = String.Format("Add {0} item{1} to Order", draggedItems.Count, draggedItems.Count > 1 ? "s" : String.Empty);

                cue.IsDropPossible = true;
                //order.Background = this.Resources["DropPossibleBackground"] as Brush;
            }
            if (e.Options.Status == DragStatus.DropComplete)
            {
                var draggedObject = e.Options.Payload as IDraggable;
                var destination = e.Options.Destination.DataContext as IDraggable;

                destination.RecieveDraggable(draggedObject);
            }
            e.Handled = true;
        }

        private void OnDropQuery(object sender, DragDropQueryEventArgs e)
        {
            // Ask the destination if dropping is possible
            if (e.Options.Status == DragStatus.DropDestinationQuery)
            {
                // Check whether the source and the target are not the same
                if (this != e.Options.Source)
                {
                    //DragRequest request = e.Options.Payload as DragRequest;
                    // Check whether the control where the dragged object comes from is
                    // different than the one we are dropping the object onto.
                    //e.QueryResult = (request.ItemsHost != this);
                    var destination = e.Options.Payload;
                    e.QueryResult = true;
                    //if (e.QueryResult == true)
                    //{
                    //    e.Handled = true;
                    //}
                }
            }
        }

        private void OnDragQuery(object sender, DragDropQueryEventArgs e)
        {
            // An object is about the be dragged
            if (e.Options.Status == DragStatus.DragQuery)
            {
                // Set result to true
                e.QueryResult = true;
                // Get the string representation of the item that is being dragged
                var draggedItem = (ListBoxItem) e.Options.Source;
                // Put a new instance of the DragRequest class within Payload
                // The DragRequest instance contains the string representation of the object being dragged,
                // as well as the origin the the object
                e.Options.Payload = draggedItem.DataContext;

                //e.Source = this;
                //e.Handled = true;
            }
            // Ask if it is OK to drop the target at the particular location
            if (e.Options.Status == DragStatus.DropSourceQuery)
            {
                e.QueryResult = true;
                e.Handled = true;
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            RadDragAndDropManager.SetAllowDrag(element, true);
        }
    }
}