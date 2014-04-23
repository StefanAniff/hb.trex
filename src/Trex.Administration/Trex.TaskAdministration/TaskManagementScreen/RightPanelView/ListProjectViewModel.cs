using System.Collections.Generic;
using System.Linq;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public class ListProjectViewModel : ListItemModelBase
    {
        public ListProjectViewModel(Project project, ListItemModelBase parent, IDataService dataService, IUserRepository userRepository)
            : base(parent, project, dataService, userRepository)
        {
            ActionPanelViewFactory = new ProjectActionPanelViewFactory(project);
            InitUserName();
        }

        private void InitUserName()
        {
            var creator = UserRepository.GetById(Project.CreatedBy);
            UserName = creator != null ? creator.Name : string.Empty;
        }

        public Project Project
        {
            get { return (Project)Entity; }
        }

        public override string DisplayName
        {
            get { return Project.ProjectName + VisibleChildrenCountString; }
        }

        public override double TotalBillableTime
        {
            get
            {
                return Children.OfType<ListTaskViewModel>().Sum(taskViewModel => taskViewModel.TotalBillableTime);
            }
        }

        public override double TotalTimeSpent
        {
            get
            {
                return Children.OfType<ListTaskViewModel>().Sum(taskViewModel => taskViewModel.TotalTimeSpent);
            }
            set
            {
                base.TotalTimeSpent = value;
            }
        }

        public string TotalBillableTimeSpent
        {
            get { return TotalBillableTime.ToString("N2"); }
        }

        public override double TotalTimeLeft
        {
            get
            {

                return Children.Sum(task => ((ListTaskViewModel)task).Task.TimeLeft);
            }
        }

        public override double TotalProgress
        {
            get
            {

                var totalEstimate = Children.Sum(task => ((ListTaskViewModel)task).Task.TimeEstimated);
                if (!totalEstimate.Equals(0))
                {
                    return (TotalBillableTime * 100 / totalEstimate);
                }

                return 0;
            }
        }

        public override double TotalRealisticProgress
        {
            get
            {
                var totalEstimate = Children.Sum(task => ((ListTaskViewModel)task).Task.TimeEstimated);
                var totalTimeLeft = Children.Sum(task => ((ListTaskViewModel)task).Task.TimeLeft);

                if (!totalEstimate.Equals(0))
                {
                    return ((1 - (totalTimeLeft / totalEstimate)) * 100);
                }

                return 0;
            }
        }

        public override double TotalEstimate
        {
            get
            {
                //return 0;
                double total = 0;
               
                foreach (var child in Children)
                {
                    var taskViewModel = child as ListTaskViewModel;
                    if (taskViewModel != null)
                    {
                        total += taskViewModel.Task.TimeEstimated;
                    }
                }
                return total;
            }
        }

        public override string Label
        {
            get
            {
                return Project.CustomerInvoiceGroup != null ? Project.CustomerInvoiceGroup.Label : string.Empty;
            }
            set
            {
                base.Label = value;
            }
        }

        public override System.DateTime CreateDate
        {
            get { return Project.CreateDate; }
            set
            {
                base.CreateDate = value;
            }
        }

        public bool IsEstimatesEnabled
        {
            get { return Project.IsEstimatesEnabled; }
        }

        public IEnumerable<ListTaskViewModel> Tasks
        {
            get { return Children.Cast<ListTaskViewModel>(); }
        }

        public override void Reload()
        {
            base.Update();
            Parent.Reload();
        }

        public override void RemoveChild(int childId)
        {
            var taskViewToRemove = Children.SingleOrDefault(task => ((ListTaskViewModel)task).Task.Id == childId);
            if (taskViewToRemove != null)
            {
                Children.Remove(taskViewToRemove);
            }

            Reload();
        }

        public override ListItemModelBase AddChild(IEntity entity)
        {

            var viewModel = new ListTaskViewModel(entity as Task, this, DataService, UserRepository);
            Children.Add(viewModel);

            Reload();
            return viewModel;
        }

        public override bool CanPass(Core.Model.TimeEntryFilter filter, TreeItemSelectionFilter selectionFilter)
        {
            return selectionFilter.HasProject(Project)
                    && !(filter != null && filter.HideEmptyProjects && !Children.Any(x => x.CanPass(filter, selectionFilter)));
        }
    }
}