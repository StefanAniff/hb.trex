using System;
using System.Windows;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class CommonDialogs : ICommonDialogs
    {
        public bool ContinueWarning(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }

        public void Ok(string message, string caption, MessageBoxImage messageBoxImage)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, messageBoxImage);
        }

        public void Error(string message)
        {
            Ok(message, "Error", MessageBoxImage.Error);
        }

        public void Error(string title, string message, Exception exception)
        {
            var exceptionBox = new ExceptionWindow();
            exceptionBox.ShowException(title, message, exception);
        }
    }
}