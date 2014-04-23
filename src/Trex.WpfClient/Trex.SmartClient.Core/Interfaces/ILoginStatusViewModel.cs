using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface ILoginStatusViewModel
    {
        string UserName { get; }
        string ButtonText { get; }
        bool IsVisible { get; }
        DelegateCommand<object> LogOut { get; set; }
        DelegateCommand<object> ChangePassword { get; set; }
    }
}
