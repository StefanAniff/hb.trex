using System.IO;
using System.IO.IsolatedStorage;
using Trex.Core.Services;
using Trex.Core.Utils;

namespace Trex.Infrastructure.Implemented
{
    public class IsolatedStorageUserDataService : IUserSettingsService
    {
        private const string STORAGE_FILE_NAME = "userData";

        #region IUserSettingsService Members

        public ILoginSettings GetSettings()
        {
            LoginSettings loginSettings = null;
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFile.FileExists(STORAGE_FILE_NAME))
                {
                    using (var storageFileStream = new IsolatedStorageFileStream(STORAGE_FILE_NAME, FileMode.Open, storageFile))
                    {
                        using (var sr = new StreamReader(storageFileStream))
                        {
                            var data = sr.ReadToEnd();
                            loginSettings =
                                SerializationUtils.DeSerialize(data, typeof (LoginSettings)) as LoginSettings;
                        }
                    }
                }
            }
            return loginSettings;
        }

        public void SaveSettings(ILoginSettings loginSettings)
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var storageFileStream = new IsolatedStorageFileStream(STORAGE_FILE_NAME, FileMode.Create, storageFile))
                {
                    using (var streamWriter = new StreamWriter(storageFileStream))
                    {
                        streamWriter.Write(SerializationUtils.Serialize(loginSettings));
                        streamWriter.Close();
                    }
                }
            }
        }

        public void DeleteSettings()
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var storageFileStream = new IsolatedStorageFileStream(STORAGE_FILE_NAME, FileMode.Truncate, storageFile)) {}
            }
        }

        #endregion
    }
}