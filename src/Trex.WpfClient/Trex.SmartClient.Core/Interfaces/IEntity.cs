namespace Trex.SmartClient.Core.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        bool IsValidChild(IEntity entity);
        
    }
}
