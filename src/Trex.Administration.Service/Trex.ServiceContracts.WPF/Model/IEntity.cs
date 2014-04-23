namespace Trex.ServiceContracts
{
    public interface IEntity
    {
        int Id { get; set; }
        bool IsValidChild(IEntity entity);
    }
}