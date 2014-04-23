using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Service
{
    public interface IServiceStackClient
    {
        T Get<T>(IReturn<T> request);
        Task AuthorizeAsync(ILoginSettings loginSettings);
        Task<T> GetAsync<T>(IReturn<T> request);
        Task<T> PostAsync<T>(IReturn<T> request);
        void Authorize(ILoginSettings loginSettings);
    }
}