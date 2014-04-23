using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Extensions;
using log4net;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class ApplicationStateService : IApplicationStateService, IDisposable
    {
        private readonly IIsolatedStorageFileProvider _isolatedStorageFileProvider;
        private readonly ApplicationState _applicationState;
        private static readonly string OpenTimeEntriesXmlFile = appSettings.Default.OpenTimeEntriesXmlFile;
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object LockObj = new object();
        private readonly IDisposable _saveSubscription;

        public ApplicationStateService(IIsolatedStorageFileProvider isolatedStorageFileProvider)
        {
            _isolatedStorageFileProvider = isolatedStorageFileProvider;

            _applicationState = GetState();
            _applicationState.PropertyChanged += _applicationState_PropertyChanged;
            _applicationState.OpenTimeEntries.CollectionChanged += OpenTimeEntries_CollectionChanged;

            _saveSubscription = Observable.Interval(TimeSpan.FromMinutes(5)).Subscribe(x => Save());
        }

        private void OpenTimeEntries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Save();
        }

        private void _applicationState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();
        }


        public void AddOpenTimeEntry(TimeEntry timeEntry)
        {
            _applicationState.OpenTimeEntries.Add(timeEntry.Copy());
        }

        public void RemoveOpenTimeEntry(TimeEntry timeEntry)
        {
            var itemToRemove = _applicationState.OpenTimeEntries.FirstOrDefault(te => te.Guid == timeEntry.Guid);
            if (itemToRemove != null)
            {
                _applicationState.OpenTimeEntries.Remove(itemToRemove);
            }
        }


        public void Save()
        {
            ThreadPool.QueueUserWorkItem(s => ExecuteSave());
        }


        private void ExecuteSave()
        {
            lock (LockObj)
            {
                try
                {
                    var appScope = _isolatedStorageFileProvider.GetIsolatedStorage();
                    using (var fs = new IsolatedStorageFileStream(OpenTimeEntriesXmlFile, FileMode.Create, appScope))
                    {
                        var formatter = new XmlSerializer(typeof (ApplicationState));
                        formatter.Serialize(fs, _applicationState);
                        var sb = new StringBuilder();
                        TextWriter fwriter = new StringWriter(sb);
                        formatter.Serialize(fwriter, _applicationState);
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        private ApplicationState GetState()
        {

            lock (LockObj)
            {

                var appScope = _isolatedStorageFileProvider.GetIsolatedStorage();
                var formatter = new XmlSerializer(typeof (ApplicationState));

                try
                {
                    using (var fs = new IsolatedStorageFileStream(OpenTimeEntriesXmlFile, FileMode.OpenOrCreate, appScope))
                    {
                        return (ApplicationState) formatter.Deserialize(fs);
                    }
                }
                catch (Exception)
                {
                    using (var fs = new IsolatedStorageFileStream(OpenTimeEntriesXmlFile, FileMode.Create, appScope))
                    {
                        formatter.Serialize(fs, new ApplicationState());
                    }
                    return new ApplicationState();
                }
            }
        }


        public ApplicationState CurrentState
        {
            get
            {
                lock (LockObj)
                {
                    return _applicationState;
                }
            }
        }

        public void Dispose()
        {
            _saveSubscription.Dispose();
        }
    }
}