using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IVersionRepository
    {
        DBVersion GetCurrentVersion();
    }
}