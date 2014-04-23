using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Trex.TaskAdministration.Commands
{
    public static class LoadOnDemand
    {
        // Using a DependencyProperty as the backing store for LoadonDemandBehavior.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty LoadOnDemandCommandBehaviorProperty =
            DependencyProperty.RegisterAttached("LoadOnDemandCommandBehavior", typeof (LoadOnDemandBehaviour), typeof (LoadOnDemand), null);

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter", typeof (object), typeof (LoadOnDemand), new PropertyMetadata(OnSetCommandParameterCallback));

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (LoadOnDemand), new PropertyMetadata(OnSetCommandCallback));

        private static LoadOnDemandBehaviour GetOrCreateBehavior(DependencyObject element)
        {
            var behavior = element.GetValue(LoadOnDemandCommandBehaviorProperty) as LoadOnDemandBehaviour;
            var specificControl = element as RadTreeView;
            if (behavior == null)
            {
                behavior = new LoadOnDemandBehaviour(specificControl);
                element.SetValue(LoadOnDemandCommandBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static LoadOnDemandBehaviour GetLoadOnDemandCommandBehavior(DependencyObject obj)
        {
            return (LoadOnDemandBehaviour) obj.GetValue(LoadOnDemandCommandBehaviorProperty);
        }

        public static void SetLoadOnDemandCommandBehavior(DependencyObject obj, LoadOnDemandBehaviour value)
        {
            obj.SetValue(LoadOnDemandCommandBehaviorProperty, value);
        }

        #region Never Changes

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(DependencyObject element)
        {
            return element.GetValue(LoadOnDemandCommandBehaviorProperty);
        }

        public static void SetCommandParameter(DependencyObject element, object parameter)
        {
            element.SetValue(CommandParameterProperty, parameter);
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var behavior = GetOrCreateBehavior(dependencyObject);
            behavior.Command = e.NewValue as ICommand;
        }

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var behaviour = GetOrCreateBehavior(dependencyObject);
            behaviour.CommandParameter = e.NewValue;
        }

        #endregion
    }
}