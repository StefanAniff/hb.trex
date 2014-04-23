using System.Collections.Generic;
using System.Linq;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public class ListCustomerViewModel : ListItemModelBase
    {
        public ListCustomerViewModel(Customer customer, ListItemModelBase parent, IDataService dataService,IUserRepository userRepository)
            : base(parent, customer, dataService,userRepository)
        {
            ActionPanelViewFactory = new CustomerActionPanelViewFactory(customer);
        }

        public Customer Customer
        {
            get { return (Customer) Entity; }
        }

        public override string DisplayName
        {
            get { return Customer.CustomerName + VisibleChildrenCountString; }
        }

        public override System.DateTime CreateDate
        {
            get
            {
                return Customer.CreateDate;
            }
            set
            {
                base.CreateDate = value;
            }
        }

        public override string UserName
        {
            get
            {
                var user = UserRepository.GetById(Customer.CreatedBy);
                return user != null ? user.Name : string.Empty;
            }
            set
            {
                base.UserName = value;
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override double TotalBillableTime
        {
            get
            {
                
                return Children.OfType<ListProjectViewModel>().Sum(p=>p.TotalBillableTime);
            }
            set
            {
                base.TotalBillableTime = value;
            }
        }

        public override double TotalTimeSpent
        {
            get
            {
                return Children.OfType<ListProjectViewModel>().Sum(p => p.TotalTimeSpent);
            }
            set
            {
                base.TotalTimeSpent = value;
            }
        }

        public IEnumerable<ListProjectViewModel> Projects
        {
            get { return Children.Cast<ListProjectViewModel>(); }
        }

        public override ListItemModelBase AddChild(IEntity entity)
        {
            var viewModel = new ListProjectViewModel(entity as Project, this, DataService, UserRepository);
            Children.Add(viewModel);
            Reload();
            return viewModel;

            //if (Customer.Projects.FirstOrDefault(p => p.Id == ((Project) entity).Id) == null)
            //{
            //    Customer.Projects.Add(entity as Project);
            //    Customer.AcceptChanges();
            //}

            
        }

        public override void RemoveChild(int childId)
        {

            ////Remove project itself
            //var projectToRemove = Customer.Projects.SingleOrDefault(t => t.Id == childId);
            //if (projectToRemove != null)
            //{
            //    Customer.Projects.Remove(projectToRemove);
            //}
            //Remove View
            var projectViewToRemove = Children.SingleOrDefault(project => ((ListProjectViewModel) project).Project.Id == childId);
            if (projectViewToRemove != null)
            {
                Children.Remove(projectViewToRemove);
            }

         

            Reload();
        }

        public override void Reload()
        {
            Update();
        }

        public override bool CanPass(Core.Model.TimeEntryFilter filter, TreeItemSelectionFilter selectionFilter)
        {
            return selectionFilter.HasCustomer(Customer);
        }
    }
}