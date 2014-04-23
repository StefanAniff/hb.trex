using Trex.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.TaskAdministration.Interfaces
{
    public interface IDraggable
    {
        IEntity Entity { get; set; }

        void RecieveDraggable(IDraggable draggable);

        void RemoveFromSource();
    }
}