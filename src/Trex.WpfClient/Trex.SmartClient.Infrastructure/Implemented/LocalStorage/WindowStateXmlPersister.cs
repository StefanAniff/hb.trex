using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class WindowStateXmlPersister : IWindowStatePersister
    {
        private readonly string _openTaskControlsFile;
        private readonly string _dataStoragePath;

        public WindowStateXmlPersister()
        {
            _dataStoragePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, appSettings.Default.DataStoragePath);
            _openTaskControlsFile = string.Concat(_dataStoragePath, appSettings.Default.OpenTimeEntriesXmlFile);

            EnsureDataStoragePath();
        }

        public List<TaskControlState> GetOpenTaskControls()
        {

            if (File.Exists(_openTaskControlsFile))
            {
                using (var file = File.OpenRead(_openTaskControlsFile))
                {
                    var serializer = new XmlSerializer(typeof(List<TaskControlState>));
                    return (List<TaskControlState>)serializer.Deserialize(file);
                }
            }

            return new List<TaskControlState>();
        }

        public void SaveOpenTaskControls(List<TaskControlState> controlStates)
        {
            using (var file = File.Create(_openTaskControlsFile))
            {
                var serializer = new XmlSerializer(controlStates.GetType());
                serializer.Serialize(file, controlStates);
            }
        }

        public void ClearOpenTasks()
        {
            if (File.Exists(_openTaskControlsFile))
            {
                File.Delete(_openTaskControlsFile);
            }
        }

        private void EnsureDataStoragePath()
        {
            if (!Directory.Exists(_dataStoragePath))
            {
                Directory.CreateDirectory(_dataStoragePath);
            }
        }
    }
}
