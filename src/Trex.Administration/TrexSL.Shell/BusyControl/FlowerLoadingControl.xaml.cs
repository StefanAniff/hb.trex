using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Trex.Core.Interfaces;

namespace TrexSL.Shell.BusyControl
{
    public partial class FlowerLoadingControl : UserControl, IBusyView
    {
        #region Constructors

        public FlowerLoadingControl()
        {
            // Required to initialize variables
            InitializeComponent();
            ShowStoryboardAnimation = Resources["ShowStoryBoard"] as Storyboard; // this.ShowStoryboard; // Also part of the Visibility Pattern
            HideStoryboardAnimation = Resources["HideStoryboard"] as Storyboard; //this.HideStoryboard; // Also part of the Visibility Pattern
            Loaded += FlowerLoadingControl_Loaded;
            var color = new Color {A = 255, R = 0, B = 0, G = 0};
            FontBrush = new SolidColorBrush(color);
            PetalBrush = new SolidColorBrush(color);
        }

        #endregion

        #region Visibility Pattern

        private Storyboard hideStoryboardAnimation;
        private Visibility visibility;

        public new Visibility Visibility
        {
            get { return visibility; }
            set
            {
                if (visibility != value)
                {
                    visibility = value;
                    OnVisibilityChanged();
                }
            }
        }

        protected Storyboard HideStoryboardAnimation
        {
            get { return hideStoryboardAnimation; }
            set
            {
                if (hideStoryboardAnimation != value)
                {
                    if (hideStoryboardAnimation != null)
                    {
                        hideStoryboardAnimation.Completed -= HideStoryboardAnimation_Completed;
                    }
                    if (value != null)
                    {
                        hideStoryboardAnimation = value;
                        hideStoryboardAnimation.Completed += HideStoryboardAnimation_Completed;
                    }
                }
            }
        }

        protected Storyboard ShowStoryboardAnimation { get; set; }

        public event EventHandler VisibilityChanged;

        protected void OnVisibilityChanged()
        {
            if (visibility == Visibility.Visible)
            {
                base.Visibility = Visibility;
                if (ShowStoryboardAnimation != null)
                {
                    ShowStoryboardAnimation.Begin();
                }
            }
            else
            {
                if (HideStoryboardAnimation != null)
                {
                    HideStoryboardAnimation.Begin();
                }
                else
                {
                    base.Visibility = Visibility;
                }
            }
            if (VisibilityChanged != null)
            {
                VisibilityChanged(this, new EventArgs());
            }
        }

        private void HideStoryboardAnimation_Completed(object sender, EventArgs e)
        {
            base.Visibility = Visibility;
        }

        #endregion

        #region Event Handlers

        private void FlowerLoadingControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DataContext = this;
            // (Application.Current.Resources["BaseColorBrush"] as SolidColorBrush).SetValue(SolidColorBrush.ColorProperty, PetalBrush); // nope
            var loadingStoryBoard = Resources["LoadingStoryboard"] as Storyboard;
            loadingStoryBoard.Begin();
        }

        #endregion

        #region PetalBrush Dependency Property

        public static readonly DependencyProperty PetalBrushProperty =
            DependencyProperty.Register(
                "PetalBrush",
                typeof (Brush),
                typeof (FlowerLoadingControl),
                new PropertyMetadata(new SolidColorBrush {Color = Color.FromArgb(255, 16, 73, 120)}, OnPetalBrushChanged));

        public Brush PetalBrush
        {
            get { return (Brush) GetValue(PetalBrushProperty); }
            set { SetValue(PetalBrushProperty, value); }
        }

        private static void OnPetalBrushChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var source = (FlowerLoadingControl) sender;
            var newBrush = (Brush) args.NewValue;

            source.centerCircle.Fill = newBrush;
            source.ellipse0.Fill = newBrush;
            source.ellipse1.Fill = newBrush;
            source.ellipse2.Fill = newBrush;
            source.ellipse3.Fill = newBrush;
            source.ellipse4.Fill = newBrush;
            source.ellipse5.Fill = newBrush;
            source.ellipse6.Fill = newBrush;
            source.ellipse7.Fill = newBrush;
            source.ellipse8.Fill = newBrush;
            source.ellipse9.Fill = newBrush;
            source.ellipse10.Fill = newBrush;
            source.ellipse11.Fill = newBrush;
            source.ellipse12.Fill = newBrush;
            source.ellipse13.Fill = newBrush;
            source.ellipse14.Fill = newBrush;
            source.ellipse15.Fill = newBrush;
            //TODO: PAPA - huh? Why do I need to do this? The DP should be working!!!
        }

        #endregion

        #region FontBrush Dependency Property

        public static readonly DependencyProperty FontBrushProperty =
            DependencyProperty.Register(
                "FontBrush",
                typeof (Brush),
                typeof (FlowerLoadingControl),
                new PropertyMetadata(new SolidColorBrush {Color = Color.FromArgb(255, 255, 255, 255)}, OnFontBrushChanged));

        public Brush FontBrush
        {
            get { return (Brush) GetValue(FontBrushProperty); }
            set { SetValue(FontBrushProperty, value); }
        }

        private static void OnFontBrushChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var source = (FlowerLoadingControl) sender;
            var newBrush = (Brush) args.NewValue;

            source.caption.Foreground = newBrush;
            //TODO: PAPA - huh? Why do I need to do this? The DP should be working!!!
        }

        #endregion

        #region Caption Dependency Property

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register(
                "Caption",
                typeof (string),
                typeof (FlowerLoadingControl),
                new PropertyMetadata("Loading ...", OnCaptionChanged));

        public string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        private static void OnCaptionChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var source = (FlowerLoadingControl) sender;
            var newCaption = (string) args.NewValue;

            source.caption.Text = newCaption;
            //TODO: PAPA - huh? Why do I need to do this? The DP should be working!!!
        }

        #endregion

        #region IBusyView Members

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        public string Message
        {
            get { return Caption; }
            set { Caption = value; }
        }

        #endregion
    }
}