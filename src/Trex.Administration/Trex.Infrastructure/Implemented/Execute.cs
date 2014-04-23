using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Trex.Infrastructure.Implemented
{
    public static class Execute
    {
        /// <summary>
        /// Invokes the action the UI thread with the provided priority
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        public static void InUIThread(Action action)
        {


            Deployment.Current.Dispatcher.BeginInvoke(action);
                
        }

      

        /// <summary>
        /// Invokes the action in a background thread
        /// </summary>
        /// <param name="action">The action.</param>
        public static void InBackground(Action action)
        {
            InBackground(action, null);
        }


        /// <summary>
        /// Invokes the action in a background thread, and calls the callback action when completed
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callback">The callback.</param>
        public static void InBackgroundSingle(Action action, Action callback)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            //if (callback != null)
            //{

            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            //}

            backgroundWorker.RunWorkerAsync(new Action[] { action, callback });
        }

        /// <summary>
        /// Invokes the action in a background thread, and calls the callback action when completed
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callback">The callback.</param>
        public static void InBackground(Action action, Action callback)
        {
           
                //Only start background thread, if we are on the ui thread
                if (Deployment.Current.Dispatcher.CheckAccess())
                    ThreadPool.QueueUserWorkItem(Dowork, new AsyncHelperClass { WorkToDo = action, CompletedAction = callback });
                //otherwise simply call the action
                else
                {
                    action();
                    if (callback != null)
                        InUIThread(callback);
                }
           
        }

        private static void Dowork(object state)
        {
            var asyncHelper = (AsyncHelperClass)state;

            asyncHelper.WorkToDo();
            if (asyncHelper.CompletedAction != null)
                Execute.InUIThread(() => asyncHelper.CompletedAction());
        }

        static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw e.Error;
            }
            var callback = e.Result as Action;
            if (callback != null)
            {
                callback();
            }
        }

        static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            Action[] actions = e.Argument as Action[];
            if (actions != null && actions.Length > 0)
            {
                Action work = actions[0];

                if (work != null)
                {
                    work();

                }

                if (actions.Length > 1)
                {
                    Action callback = actions[1];
                    e.Result = callback;
                }
            }
        }

        private class AsyncHelperClass
        {
            public Action WorkToDo { get; set; }
            public Action CompletedAction { get; set; }
        }


    }

}
