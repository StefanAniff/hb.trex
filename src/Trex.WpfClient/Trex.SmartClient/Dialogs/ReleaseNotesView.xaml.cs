using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Trex.SmartClient.Dialogs
{
    /// <summary>
    /// Interaction logic for ReleaseNotesView.xaml
    /// </summary>
    public partial class ReleaseNotesView : UserControl
    {
        public ReleaseNotesView()
        {
            InitializeComponent();
        }

        private void Pbutton_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Pbutton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
    }
}
