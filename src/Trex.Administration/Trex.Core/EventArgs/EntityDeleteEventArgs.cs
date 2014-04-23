namespace Trex.Core.EventArgs
{
    public class EntityDeleteEventArgs : System.EventArgs
    {
        public EntityDeleteEventArgs(int entityDeletedId)
        {
            EntityDeletedId = entityDeletedId;
        }

        public int EntityDeletedId { get; set; }
    }
}