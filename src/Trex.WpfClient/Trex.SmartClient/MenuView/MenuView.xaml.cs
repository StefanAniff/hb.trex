using System;
using System.Windows.Controls;
using System.Windows.Input;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.MenuView
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl,IView
    {
        public MenuView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            this.DataContext = viewModel;
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
