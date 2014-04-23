using System;
using System.Windows;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface ICommonDialogs
    {
        bool ContinueWarning(string message, string caption);
        void Ok(string message, string caption, MessageBoxImage messageBoxImage);
        void Error(string message);
        void Error(string title, string message, Exception exception);
    }
}