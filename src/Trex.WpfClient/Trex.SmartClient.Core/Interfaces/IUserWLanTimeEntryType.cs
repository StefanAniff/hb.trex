namespace Trex.SmartClient.Core.Interfaces
{
    public interface IUserWLanTimeEntryType
    {
        string WifiName { get; set; }
        int DefaultTimeEntryTypeId { get; set; }
        bool Connected { get; set; }
    }
}