using Trex.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.TaskAdministration.EventArgs
{
    public class MoveEntityEventArgs : System.EventArgs
    {
        public MoveEntityEventArgs(IEntity movedEntity, IEntity oldParent, IEntity newParent)
        {
            MovedEntity = movedEntity;
            OldParent = oldParent;
            NewParent = newParent;
        }

        public IEntity MovedEntity { get; set; }
        public IEntity OldParent { get; set; }
        public IEntity NewParent { get; set; }
    }
}