using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Data;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage.Forecast
{
    public class ForecastDataSetWrapper : IForecastDataSetWrapper
    {
        private readonly string _fileName = appSettings.Default.ForecastDataFile;
        private ForecastDataSet _forecastDataSet;
        private readonly IIsolatedStorageFileProvider _isolatedStorageFileProvider;

        public ForecastDataSetWrapper(IIsolatedStorageFileProvider isolatedStorageFileProvider)
        {
            _isolatedStorageFileProvider = isolatedStorageFileProvider;
        }

        public ForecastDataSet ForecastDataSet
        {
            get
            {
                if (_forecastDataSet == null)
                    Load();
                return _forecastDataSet;
            }
        }

        public void Save()
        {
            var isoStorage = _isolatedStorageFileProvider.GetIsolatedStorage();
            using (var fs = new IsolatedStorageFileStream(_fileName, FileMode.OpenOrCreate, isoStorage))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, ForecastDataSet);
            }
        }

        public void Load()
        {
            var isoStorage = _isolatedStorageFileProvider.GetIsolatedStorage();
            if (!isoStorage.FileExists(_fileName))
                _forecastDataSet = new ForecastDataSet();

            using (var fs = new IsolatedStorageFileStream(_fileName, FileMode.OpenOrCreate, isoStorage))
            {
                if (fs.Length == 0)
                    _forecastDataSet = new ForecastDataSet();
                else
                {
                    var formatter = new BinaryFormatter();
                    _forecastDataSet = (ForecastDataSet)formatter.Deserialize(fs);                    
                }
            }
        }

        public void DeleteFile()
        {
            var isoStorage = _isolatedStorageFileProvider.GetIsolatedStorage();
            if (isoStorage.FileExists(_fileName))
            {
                isoStorage.DeleteFile(_fileName);
            }
        }
    }

    public interface IForecastDataSetWrapper
    {
        void Save();
        void Load();
        ForecastDataSet ForecastDataSet { get; }
    }
}