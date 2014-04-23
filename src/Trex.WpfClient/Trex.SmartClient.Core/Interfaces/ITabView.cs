using System.Windows.Controls;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface ITabView : IView
    {
        RowDefinition HostRow { get; }
    }
}