using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.Dialog.SelectTask
{
    /// <summary>
    /// Interaction logic for SelectTaskView.xaml
    /// </summary>
    public partial class SelectTaskView : UserControl, IView
    {
        public static readonly DependencyProperty CanCancelProperty = DependencyProperty.Register("CanCancel", typeof (bool), typeof (SelectTaskView), new PropertyMetadata(true));

        public SelectTaskView()
        {
            InitializeComponent();
            Loaded += SelectTaskView_Loaded;

            TreeSelector.KeyUp += taskListBox_KeyUp;
            ListSelector.KeyUp += taskListBox_KeyUp;

            Observable.FromEventPattern(TxtSearchBox, "TextChanged")
                      .Select(e => ((TextBox) e.Sender).Text)
                      .Where(text => text.Length > 2 || text == "*")
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .ObserveOnDispatcher()
                      .Subscribe(x =>
                          {
                              //raise binding explicitly
                              var be = TxtSearchBox.GetBindingExpression(TextBox.TextProperty);
                              if (be != null) be.UpdateSource();
                          });

        }

        public bool CanCancel
        {
            get { return (bool) GetValue(CanCancelProperty); }
            set { SetValue(CanCancelProperty, value); }
        }


        private void SelectTaskView_Loaded(object sender, RoutedEventArgs e)
        {
            TxtSearchBox.Focus();
        }


        private void taskListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectButton.Command.CanExecute(null))
                {
                    SelectButton.Command.Execute(null);
                }
            }

        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}