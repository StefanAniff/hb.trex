
namespace Trex.Server.Infrastructure.State
{
    public interface IRequestState
    {
        T Get<T>(string key);
        void Store(string key, object something);
    }
}