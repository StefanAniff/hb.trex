using System;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Project.TaskDisposition;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    public class ProjectAdministrationViewModel : ViewModelBase, IProjectAdministrationViewModel
    {
        public DelegateCommand<object> GotoProjectDispositionCommand { get; private set; }

        public ProjectAdministrationViewModel()
        {
            InitializeCommands();
        }


        #region Commandhandlers

        private void InitializeCommands()
        {
            GotoProjectDispositionCommand = new DelegateCommand<object>(GotoProjectDispositionExecute,
                                                                        CanExecuteGotoProjectDisposition);
        }

        private bool CanExecuteGotoProjectDisposition(object arg)
        {
            // IVA: Do me
            return true;
        }

        private void GotoProjectDispositionExecute(object obj)
        {
            ApplicationCommands.JumpToSubmenuCommand.Execute(new JumpToSubmenuParam { ViewType = typeof(TaskDispositionView), InitializationParam = null });
        }

        #endregion

    }
}