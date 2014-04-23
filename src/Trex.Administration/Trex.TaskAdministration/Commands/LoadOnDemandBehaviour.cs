using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace Trex.TaskAdministration.Commands
{
    public class LoadOnDemandBehaviour : CommandBehaviorBase<Control>
    {
        public LoadOnDemandBehaviour(RadTreeView targetObject)
            : base(targetObject)
        {
            if (targetObject != null)
            {
                targetObject.LoadOnDemand += OnLoadOnDemand;
            }
        }

        private void OnLoadOnDemand(object sender, RadRoutedEventArgs e)
        {
            ExecuteCommand();
        }
    }
}