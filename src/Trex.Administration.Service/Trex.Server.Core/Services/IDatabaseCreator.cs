namespace Trex.Server.Core.Services
{
    public interface IDatabaseCreator
    {
        void CreateTrex(string customerId);
        string ConnectionString { get; }
    }
}
