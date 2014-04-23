using System.Windows;
using System.Windows.Controls;
using Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel;

namespace Trex.Dialog.Implemented
{
    public class TaskListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TaskItemViewmodel { get; set; }
        public DataTemplate SearchOnServerTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is TaskListViewModel)
            {
                return TaskItemViewmodel;
            }
            else if (item is SearchOnServerTaskViewmodel)
            {
                return SearchOnServerTemplate;
            }

            return null;
        }
    }
}
