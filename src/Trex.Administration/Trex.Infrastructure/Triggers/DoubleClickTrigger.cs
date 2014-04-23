using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Trex.Infrastructure.Triggers
{
    public class DoubleClickTrigger : TriggerBase<UIElement>
    {
        private readonly DispatcherTimer _timer;

        public DoubleClickTrigger()
        {
            _timer = new DispatcherTimer
                         {
                             Interval = new TimeSpan(0, 0, 0, 0, 200)
                         };
            _timer.Tick += OnTimerTick;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += OnMouseButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= OnMouseButtonDown;
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        private void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_timer.IsEnabled)
            {
                _timer.Start();
                return;
            }

            _timer.Stop();
            InvokeActions(null);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
        }
    }
}