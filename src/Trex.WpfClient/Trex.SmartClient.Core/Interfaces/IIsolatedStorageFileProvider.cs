using System.IO.IsolatedStorage;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IIsolatedStorageFileProvider
    {
        IsolatedStorageFile GetIsolatedStorage();
    }
}