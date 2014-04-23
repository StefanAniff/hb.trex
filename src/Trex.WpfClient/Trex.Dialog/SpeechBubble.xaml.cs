using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trex.Dialog
{
    /// <summary>
    /// Interaction logic for SpeechBubble.xaml
    /// </summary>
    public partial class SpeechBubble : UserControl
    {
        public SpeechBubble()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var textBox = sender as TextBox;
                var grid = textBox.Parent as Grid;
                var popup = grid.Parent as Popup;
                popup.IsOpen = false;
            }
        }

   
        private void ThePopupsName_OnOpened(object sender, EventArgs e)
        {
            commentTxtbox.Focus();
            var count = commentTxtbox.Text.Count();
            if (count > 0)
            {
                commentTxtbox.Select(count, count);
            }
        }
    }
}
