using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Trex.Expander
{
    [TemplatePart(Name = RootElement, Type = typeof (FrameworkElement))]
    [TemplatePart(Name = HeaderContentElement, Type = typeof (ContentControl))]
    [TemplatePart(Name = ContentElement, Type = typeof (ContentControl))]
    [TemplatePart(Name = ExpanderButton, Type = typeof (FrameworkElement))]
    public class Expander : ContentControl
    {
        private const string RootElement = "RootElement";
        private const string HeaderContentElement = "HeaderContentElement";
        private const string ContentElement = "ContentElement";
        private const string HeaderPanel = "HeaderPanel";
        private const string ExpanderButton = "ExpanderButton";

        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof (FrameworkElement), typeof (Expander), null);

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof (bool), typeof (Expander), new PropertyMetadata(OnIsExpandedChanged));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof (bool), typeof (Expander), new PropertyMetadata(OnIsSelectedChanged));

        private ToggleButton _expanderButton;
        private FrameworkElement _headerElement;
        private bool _isExpanded;
        private FrameworkElement _rootElement;

        public Expander()
        {
            DefaultStyleKey = typeof (Expander);
            IsExpanded = false;
            IsSelected = false;
        }

        public FrameworkElement HeaderContent
        {
            get { return (FrameworkElement) GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootElement = GetTemplateChild(RootElement) as FrameworkElement;
            _headerElement = GetTemplateChild(HeaderPanel) as FrameworkElement;
            _expanderButton = GetTemplateChild(ExpanderButton) as ToggleButton;

            if (_rootElement == null)
            {
                return;
            }

            if (_headerElement != null)
            {
                _headerElement.MouseLeave += _headerElement_MouseLeave;
                _headerElement.MouseEnter += _headerElement_MouseEnter;
                _headerElement.MouseLeftButtonUp += _headerElement_MouseLeftButtonUp;
            }

            if (_expanderButton != null)
            {
                _expanderButton.Click += _expanderButton_Click;
            }

            InitializeState();
        }

        private void _headerElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
        }

        private void _expanderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_expanderButton.IsChecked.HasValue)
            {
                IsExpanded = false;
            }

            else
            {
                IsExpanded = _expanderButton.IsChecked.Value;
            }
        }

        private void _headerElement_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", false);
        }

        private void _headerElement_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsSelected)
            {
                VisualStateManager.GoToState(this, "MouseOut", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "MouseOutSelected", false);
            }
        }

        protected static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Expander) d).OnIsExpandedChanged((bool) e.OldValue, (bool) e.NewValue);
        }

        internal virtual void OnIsExpandedChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                VisualStateManager.GoToState(this, "Expanded", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", true);
            }
        }

        protected static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Expander) d).OnIsSelectedChanged((bool) e.OldValue, (bool) e.NewValue);
        }

        internal virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                VisualStateManager.GoToState(this, "Selected", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", false);
            }
        }

        internal void InitializeState()
        {
            if (IsSelected)
            {
                VisualStateManager.GoToState(this, "Selected", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", false);
            }

            if (IsExpanded)
            {
                VisualStateManager.GoToState(this, "Expanded", false);
                if (_expanderButton != null)
                {
                    _expanderButton.IsChecked = true;
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", false);
            }
        }
    }
}