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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Overview.SelectTaskBox
{
    /// <summary>
    /// Interaction logic for SelectTaskBox.xaml
    /// </summary>
    public partial class SelectTaskBox : UserControl, IView
    {
        public SelectTaskBox()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
