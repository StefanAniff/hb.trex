using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Interfaces;
using Trex.TaskAdministration.TaskManagementScreen.RightPanelView;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class ButtonPanelViewModel : ViewModelBase, IButtonPanelViewModel

    {
        private bool _canExportToExcel;
        private DelegateCommand<ListItemModelBase> _executeApply;


        public ButtonPanelViewModel()
        {
            ActionPanels = new ObservableCollection<IView>();
            _executeApply = new DelegateCommand<ListItemModelBase>(ApplyActionPanel);
            InternalCommands.ItemSelected.RegisterCommand(_executeApply);
            ExportToExcel = new DelegateCommand<object>(ExportToExcelStart, CanExportToExcel);
        }

        public ObservableCollection<IView> ActionPanels { get; set; }
        public DelegateCommand<object> ExportToExcel { get; set; }

        private bool CanExportToExcel(object arg)
        {
            return _canExportToExcel;
        }

        private void ExportToExcelStart(object obj)
        {
            InternalCommands.ExcelExportStart.Execute(null);
        }

        private void ApplyActionPanel(ListItemModelBase obj)
        {
            if(obj == null)
            {
                RemoveActionPanels();
                return;
            }
                
            _canExportToExcel = true;
            ExportToExcel.RaiseCanExecuteChanged();

            
            var actionPanelViewFactory = obj.ActionPanelViewFactory;
            if (actionPanelViewFactory != null)
            {
                RemoveActionPanels();
                ActionPanels.Add(actionPanelViewFactory.CreateActionPanelView());
                //OnPropertyChanged("ActionPanels");
            }
        }

        private void RemoveActionPanels()
        {
            foreach (var actionPanel in ActionPanels)
            {
                actionPanel.ViewModel.Close();
            }
            ActionPanels.Clear();
        }


        public override void  Close()
        {
            InternalCommands.ItemSelected.UnregisterCommand(_executeApply);
        }
    }
}