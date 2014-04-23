using System;
using System.Windows;
using System.Windows.Controls;

namespace Trex.Dialog
{

    [TemplatePart(Name = DialogPopup.TitleElement, Type = typeof(String))]
    public class DialogPopup : ContentControl
    {
        private const string TitleElement = "TitleElement";

        public DialogPopup()
        {
            DefaultStyleKey = typeof(DialogPopup);
            //var mouseDragElementBehavior = new MouseDragElementBehavior();
            //mouseDragElementBehavior.Attach(this);

        }



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(DialogPopup), null);





        public double BoxWidth
        {
            get { return (double)GetValue(BoxWidthProperty); }
            set { SetValue(BoxWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxWidthProperty =
            DependencyProperty.Register("BoxWidth", typeof(double), typeof(DialogPopup), null);




        public double BoxHeight
        {
            get { return (double)GetValue(BoxHeightProperty); }
            set { SetValue(BoxHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxHeightProperty =
            DependencyProperty.Register("BoxHeight", typeof(double), typeof(DialogPopup), null);




    }
}
