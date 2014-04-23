using System;
using System.Collections.ObjectModel;
using System.Linq;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.EventArgs;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public abstract class ListItemModelBase : ViewModelBase, IDraggable
    {
        private readonly IDataService _dataService;
        private readonly IUserRepository _userRepository;
        private bool _isExpanded;
        private bool _isSelected;
        private ObservableCollection<ListItemModelBase> _visibleChildren = new ObservableCollection<ListItemModelBase>();

        protected ListItemModelBase(ListItemModelBase parent, IEntity entity, IDataService dataService, IUserRepository userRepository)
        {
            Children = new ObservableCollection<ListItemModelBase>();
            Parent = parent;
            Entity = entity;
            _dataService = dataService;
            _userRepository = userRepository;            
        }

        protected IUserRepository UserRepository { get { return _userRepository; } }

        protected IDataService DataService
        {
            get { return _dataService; }
        }

        public virtual bool IsSelected
        {   
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public virtual bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// Gets or sets the action panel view factory. This will create an actionpanel view corresponding to the listitem selected
        /// </summary>
        /// <value>The action panel view factory.</value>
        public IActionPanelViewFactory ActionPanelViewFactory { get; set; }

        public ObservableCollection<ListItemModelBase> Children { get; set; }

        /// <summary>
        /// Exposes wich items are to be viewed in listview
        /// </summary>
        public ObservableCollection<ListItemModelBase> VisibleChildren
        {
            get { return _visibleChildren; }
            private set
            {
                _visibleChildren = value;
                OnPropertyChanged("VisibleChildren");
            }
        }

        /// <summary>
        /// Performance constly on large projects.
        /// Use with care
        /// </summary>
        public void AddVisibleChild(ListItemModelBase childToAdd)
        {
            if (VisibleChildren.Contains(childToAdd))
                return;

            VisibleChildren.Add(childToAdd);
        }

        public void SetAllChildrenVisibleByFilters(TimeEntryFilter timeEntryFilter, TreeItemSelectionFilter selectionFilter)
        {
            VisibleChildren = new ObservableCollection<ListItemModelBase>(Children.Where(x => x.CanPass(timeEntryFilter, selectionFilter)));
            foreach (var child in VisibleChildren)
            {
                child.SetAllChildrenVisibleByFilters(timeEntryFilter, selectionFilter);
            }
            Reload();
        }

        public virtual bool CanPass(TimeEntryFilter filter, TreeItemSelectionFilter selectionFilter)
        {
            return true;
        }

        public void SetAllChildrenNonVisible()
        {
            IsExpanded = false;
            IsSelected = false;
            foreach (var child in VisibleChildren)
            {
                child.SetAllChildrenNonVisible();
            }
            VisibleChildren = new ObservableCollection<ListItemModelBase>();
        }

        protected string VisibleChildrenCountString
        {
            get
            {
                if (VisibleChildren == null || VisibleChildren.Count == 0)
                    return string.Empty;
                return string.Format(" ({0})", VisibleChildren.Count);
            }
        }

        public ListItemModelBase Parent { get; set; }

        #region IDraggable Members

        public IEntity Entity { get; set; }

        public virtual string DisplayName { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Label { get; set; }
        public virtual double TotalEstimate { get; set; }
        public virtual double TotalTimeLeft { get; set; }
        public virtual double TotalProgress { get; set; }
        public virtual double TotalBillableTime { get; set; }
        public virtual double TotalRealisticProgress { get; set; }
        public virtual double TotalTimeSpent { get; set; }
        
        public virtual void RecieveDraggable(IDraggable draggable) {}

        public void RemoveFromSource()
        {
            Remove();
        }

        #endregion

        protected virtual void EntityDragged(MoveEntityEventArgs moveEntityEventArgs)
        {
            if (Entity.GetType() != moveEntityEventArgs.NewParent.GetType())
            {
                return;
            }

            if (moveEntityEventArgs.OldParent.Id == Entity.Id)
            {
                RemoveChild(moveEntityEventArgs.MovedEntity.Id);
            }

            if (moveEntityEventArgs.NewParent.Id == Entity.Id)
            {
                if (Children.SingleOrDefault(item => item.Entity.Id == moveEntityEventArgs.MovedEntity.Id) == null)
                {
                    AddChild(moveEntityEventArgs.MovedEntity);
                }
            }
        }

        public virtual void LoadChildren() {}

        public abstract ListItemModelBase AddChild(IEntity entity);

        public virtual void RemoveChild(int childId) {}

        public virtual void Reload() {}       

        public virtual void Remove()
        {
            if (Parent == null) 
                return;

            Parent.VisibleChildren.Remove(this);
            Parent.RemoveChild(Entity.Id);
        }        
    }
}