using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.SearchView
{
    public partial class SearchView : UserControl, IView
    {
        private readonly DispatcherTimer _timer;

        public SearchView()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += TimerTick;
            _timer.Interval = TimeSpan.FromMilliseconds(600);
            KeyUp += SearchViewKeyUp;

            InitializeComponent();
            _searchTextBox.KeyUp += SearchTextBoxKeyUp;
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void SearchViewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((SearchViewModel) DataContext).TaskResults = null;
            }
            if (e.Key == Key.Enter)
            {
                ((SearchViewModel) DataContext).SearchComplete.Execute(null);
            }
        }

        private void TimerTick(object sender, System.EventArgs e)
        {
            _timer.Stop();
            var bindingExpression = _searchTextBox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
                bindingExpression.UpdateSource();
        }

        private void SearchTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            ListBox resultList = null;
            if (TaskResultList.Visibility == Visibility.Visible)
            {
                resultList = TaskResultList;
            }
            else
            {
                resultList = ProjectResultList;
            }

            if (e.Key == Key.Down)
            {
                resultList.Focus();

                if (resultList.Items.Count > 0)
                    resultList.SelectedItem = resultList.Items[0];
                {
                    if (resultList.Items[0] is TaskSearchResultViewModel)
                    {
                        ((SearchViewModel)DataContext).SelectedTask = (TaskSearchResultViewModel)resultList.Items[0];
                    }
                    else
                    {
                        ((SearchViewModel)DataContext).SelectedProject =
                            (ProjectSearchResultViewModel)resultList.Items[0];
                    }
                }

                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                return;
            }

            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }

            _timer.Start();
        }
    }
}