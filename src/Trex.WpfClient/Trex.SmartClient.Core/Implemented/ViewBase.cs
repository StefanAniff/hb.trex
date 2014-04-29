using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public abstract class ViewBase : UserControl, IViewInitializable
    {
        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        public void InitializeViewModel()
        {
            var initializableContext = DataContext as IViewModelInitializable;
            if (initializableContext == null)
                return;

            initializableContext.Initialize();
        }
    }
}