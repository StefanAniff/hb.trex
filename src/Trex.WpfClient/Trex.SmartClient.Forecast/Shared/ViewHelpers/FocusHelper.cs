using System.Windows;
using System.Windows.Input;

namespace Trex.SmartClient.Forecast.Shared.ViewHelpers
{
    public class FocusHelper
    {
        public static void DoFocusNext(object offset)
        {
            var frameworkElement = offset as FrameworkElement;
            if (frameworkElement == null)
                return;

            var tRequest = new TraversalRequest(FocusNavigationDirection.Next);
            frameworkElement.MoveFocus(tRequest);
        } 


    }
}