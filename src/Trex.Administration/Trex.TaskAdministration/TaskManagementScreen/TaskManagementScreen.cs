using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.TaskAdministration.Interfaces;
using Trex.TaskAdministration.TaskManagementScreen.RightPanelView;
using Trex.TaskAdministration.TaskManagementScreen.TaskAdministrationMasterView;

namespace Trex.TaskAdministration.TaskManagementScreen
{
    [Screen(Name = "TaskAdministration", CanBeDeactivated = true)]
    public class TaskManagementScreen : ScreenBase
    {
        public TaskManagementScreen(IUnityContainer container, Guid guid)
            : base(guid, container) {}

        public FilterView.FilterView FilterView { get; private set; }

        public MainListView TaskGridView { get; private set; }

        public ITaskTreeView TaskTreeView { get; private set; }

        public SearchView.SearchView SearchView { get; private set; }

        public ButtonPanelView.ButtonPanelView ButtonPanelView { get; private set; }

        protected override void Initialize()
        {
            var taskAdministrationView = new TaskAdministrationMasterView.TaskAdministrationMasterView();
            var taskAdministrationViewModel = Container.Resolve<ITaskAdministrationMasterViewModel>();
            taskAdministrationView.ViewModel = taskAdministrationViewModel;
            MasterView = taskAdministrationView;

            var taskTreeView = Container.Resolve<ITaskTreeView>();
            var taskTreeViewModel = Container.Resolve<ITaskTreeViewModel>();
            taskTreeView.ViewModel = (taskTreeViewModel);

            TaskTreeView = taskTreeView;

            var taskGridView = new MainListView();
            var taskGridViewModel = Container.Resolve<ITaskGridViewModel>();
            taskGridView.ViewModel = (taskGridViewModel);
            TaskGridView = taskGridView;

            var filterViewModel = Container.Resolve<IFilterViewModel>();
            var filterView = new FilterView.FilterView();
            filterView.ViewModel = (filterViewModel);

            FilterView = filterView;

            var buttonPanelViewModel = Container.Resolve<IButtonPanelViewModel>();
            var buttonPanelView = new ButtonPanelView.ButtonPanelView();
            buttonPanelView.ViewModel = (buttonPanelViewModel);
            ButtonPanelView = buttonPanelView;

            var customerRepository = Container.Resolve<ICustomerRepository>();
            var userRepository = Container.Resolve<IUserRepository>();
            userRepository.Initialize();

            //var searchViewModel = new SearchView.SearchViewModel(customerRepository);
            //var searchView = new SearchView.SearchView();
            //searchView.ViewModel = (searchViewModel);

            //SearchView = searchView;

            var dialogService = Container.Resolve<IDialogService>();
            customerRepository.Initialize();
        }
    }
}