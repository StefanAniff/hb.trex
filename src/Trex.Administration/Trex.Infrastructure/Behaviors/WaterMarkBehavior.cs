using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Trex.Infrastructure.Behaviors
{
    public class WaterMarkBehavior : Behavior<TextBox>
    {
        private Brush _originalForegroundBrush;
        public string WaterMarkText { get; set; }

        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            AssociatedObject.Loaded += AssociatedObject_Loaded;

            _originalForegroundBrush = AssociatedObject.Foreground;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            SetWaterMark();
        }

        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            SetWaterMark();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Text == WaterMarkText)
            {
                AssociatedObject.Text = string.Empty;
            }

            AssociatedObject.SelectAll();
            AssociatedObject.Foreground = _originalForegroundBrush;
        }

        private void SetWaterMark()
        {
            AssociatedObject.Foreground = new SolidColorBrush(Colors.Gray);
            AssociatedObject.Text = WaterMarkText;
        }
    }
}