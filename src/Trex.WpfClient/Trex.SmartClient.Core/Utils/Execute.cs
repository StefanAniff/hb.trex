using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Trex.SmartClient.Core.Utils
{
    public static class ThreadUtils
    {

        /// <summary>
        /// This will BLOCK the thread until rendering is done by UI. Useful for huge binding updates.
        /// </summary>
        public static async Task<bool> WaitForUIRendering()
        {
            var tcs = new TaskCompletionSource<bool>();
            var watch = new Stopwatch();
            watch.Start();
            if (IsInUIThread())
            {
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => tcs.SetResult(true)), DispatcherPriority.ContextIdle, null);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => tcs.SetResult(true)), DispatcherPriority.ContextIdle, null);
            }
            watch.Stop();

            return await tcs.Task;
        }

        public static bool IsInUIThread()
        {
            if (Application.Current != null && Application.Current.Dispatcher != null)
            {
                return Application.Current.Dispatcher.Thread == Thread.CurrentThread;
            }
            return false;
        }

    }
    public static class Execute
    {
        public static void InUIThread(Action action)
        {
            if (Application.Current.Dispatcher.Thread != Thread.CurrentThread)
            {
                Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Normal);
            }
            else
            {
                action();
            }
        }        
    }
}
