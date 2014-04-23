using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Trex.SmartClient.Core.Utils
{
    public class EnterKeyTraversal
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static void EnterKeyDown(object sender, KeyEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element != null)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        e.Handled = true;
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        break;
                    case Key.Up:
                        e.Handled = true;
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                        break;
                    //case Key.Right:
                    //    e.Handled = true;
                    //    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                    //    break;
                    case Key.Down:
                        e.Handled = true;
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        break;
                    //case Key.Left:
                    //    e.Handled = true;
                    //    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                    //    break;
                }
            }
        }

        private static void Unloaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            element.Unloaded -= Unloaded;
            element.PreviewKeyDown -= EnterKeyDown;
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(EnterKeyTraversal),
                                                new UIPropertyMetadata(false, IsEnabledChanged));

        private static void IsEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as FrameworkElement;
            if (element == null) return;

            if ((bool)e.NewValue)
            {
                element.Unloaded += Unloaded;
                element.PreviewKeyDown += EnterKeyDown;
            }
            else
            {
                element.PreviewKeyDown -= EnterKeyDown;
            }
        }
    }
}
