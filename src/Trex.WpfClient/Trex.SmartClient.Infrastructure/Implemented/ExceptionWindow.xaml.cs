using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow()
        {
            InitializeComponent();            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            DataContext = null;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowException(string title, string message, Exception exception)
        {
            DataContext = new ExceptionWindowViewModel
                {
                    Details = exception.ToString(),
                    Message = message,
                    Title = title
                };

            ShowDialog();
        }

        public class ExceptionWindowViewModel : ViewModelBase
        {
            private string _title;
            private string _details;
            private string _message;

            public string Message
            {
                get { return _message; }
                set
                {
                    _message = value;
                    OnPropertyChanged(() => Message);
                }
            }

            public string Details
            {
                get { return _details; }
                set
                {
                    _details = value;
                    OnPropertyChanged(() => Details);
                }
            }

            public string Title
            {
                get { return _title; }
                set
                {
                    _title = value;
                    OnPropertyChanged(() => Title);
                }
            }
        }
    }
}
