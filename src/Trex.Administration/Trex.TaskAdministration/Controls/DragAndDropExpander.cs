using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.TreeView;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.Controls
{
    public class DragAndDropExpander : Expander.Expander
    {
        public DragAndDropExpander()
        {
            this.AddHandler(RadDragAndDropManager.DragQueryEvent, new EventHandler<DragDropQueryEventArgs>(OnDragQuery));
            this.AddHandler(RadDragAndDropManager.DropQueryEvent, new EventHandler<DragDropQueryEventArgs>(OnDropQuery));
            this.AddHandler(RadDragAndDropManager.DropInfoEvent, new EventHandler<DragDropEventArgs>(OnDropInfo));
            this.AddHandler(RadDragAndDropManager.DragInfoEvent, new EventHandler<DragDropEventArgs>(OnDragInfo));

            RadDragAndDropManager.SetAllowDrop(this, true);
            RadDragAndDropManager.SetAllowDrag(this, true);
        }

        private void OnDragInfo(object sender, DragDropEventArgs e)
        {
            if (e.Options.Status == DragStatus.DragInProgress)
            {
                var draggedItem = e.Options.Payload as IDraggable;
                var cue = new TreeViewDragCue();
                cue.ItemTemplate = ContentTemplate;
                var source = new List<IDraggable>();
                source.Add(draggedItem);
                cue.ItemsSource = source;
                e.Options.DragCue = cue;
            }
            e.Handled = true;
        }

        private void OnDropInfo(object sender, DragDropEventArgs e)
        {
            var destination = e.Options.Destination.DataContext as IDraggable;
            IDraggable draggedItem = null;

            if (e.Options.Payload is IEnumerable)
            {
                var enumerator = ((IEnumerable) e.Options.Payload).GetEnumerator();
                if (enumerator.MoveNext())
                {
                    draggedItem = enumerator.Current as IDraggable;
                }
            }
            else if (e.Options.Payload is IDraggable)
            {
                draggedItem = e.Options.Payload as IDraggable;
            }

            if (!IsExpanded)
            {
                IsExpanded = true;
            }

            if (e.Options.Status == DragStatus.DropPossible)
            {
                if (e.Options.DragCue is TreeViewDragCue)
                {
                    var treeViewCue = (TreeViewDragCue) e.Options.DragCue;
                    treeViewCue.DragActionContent = String.Format("Add to {0}\n", destination.Entity);
                    treeViewCue.IsDropPossible = true;
                }
            }
            if (e.Options.Status == DragStatus.DropComplete)
            {
                if (draggedItem != null && destination != null)
                {
                    destination.RecieveDraggable(draggedItem);
                }
            }
            e.Handled = true;
        }

        private void OnDropQuery(object sender, DragDropQueryEventArgs e)
        {
            // Ask the destination if dropping is possible
            if (e.Options.Status == DragStatus.DropDestinationQuery)
            {
                var source = e.Options.Payload as IDraggable;
                var destination = DataContext as IDraggable;
                if (source != null && destination != null)
                {
                    e.QueryResult = destination.Entity.IsValidChild(source.Entity);
                }

                e.Handled = true;
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
                var draggedItem = (Control) e.Options.Source;
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
    }
}