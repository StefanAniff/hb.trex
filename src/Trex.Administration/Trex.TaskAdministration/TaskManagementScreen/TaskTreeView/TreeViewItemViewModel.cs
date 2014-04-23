using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.EventArgs;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskTreeView
{
    public class TreeViewItemViewModel : ViewModelBase, IDraggable
    {
        private bool _isExpanded;
        private bool _isLoadOnDemandEnabled;
        private bool _isSelected;
        private TreeViewItemViewModel _parent;
        private ObservableCollection<TreeViewItemViewModel> _children;

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, IEntity entity)
        {
            _parent = parent;
            Entity = entity;

            Children = new ObservableCollection<TreeViewItemViewModel>();

            InternalCommands.MoveEntityRequest.RegisterCommand(new DelegateCommand<MoveEntityEventArgs>(EntityMoved));
        }

        protected SolidColorBrush ActiveColor { get { return new SolidColorBrush(Colors.Black); } }
        protected SolidColorBrush InActiveColor { get { return new SolidColorBrush(Colors.Gray); } }


        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return _children; }
            set
            {
                if (_children != null)
                    _children.CollectionChanged -= ChildrenCollectionChanged;

                _children = value;
                OnPropertyChanged("Children");

                if (_children != null)
                    _children.CollectionChanged += ChildrenCollectionChanged;
            }
        }

        private void ChildrenCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            OnPropertyChanged("DisplayName");
        }

        public bool IsLoadOnDemandEnabled
        {
            get { return _isLoadOnDemandEnabled; }

            set
            {
                if (value == _isLoadOnDemandEnabled) return;
                _isLoadOnDemandEnabled = value;
                OnPropertyChanged("IsLoadOnDemandEnabled");
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");

                if (value != _isExpanded)
                {                    
                }

                if (_isExpanded && _parent != null)
                {
                    _parent.IsExpanded = true;
                }

                if (IsLoadOnDemandEnabled)
                {
                    LoadChildren();
                }
            }
        }

        public virtual string DisplayName { get; set; }

        protected virtual string ChildrenCountDisplay { get { return Children != null && Children.Count > 0 ? " (" + Children.Count + ")" : string.Empty; } }

        public TreeViewItemViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    
                    OnPropertyChanged("IsSelected");
                }

                if (_isSelected && IsLoadOnDemandEnabled)
                {
                    LoadChildren();
                }
            }
        }

        #region IDraggable Members

        public IEntity Entity { get; set; }

        public virtual void RecieveDraggable(IDraggable draggable) {}

        public virtual void RemoveFromSource()
        {
            Remove();
        }

        #endregion

        public event EventHandler LoadChildrenCompleted;

        protected virtual void LoadChildren() {}

        public virtual void Reload()
        {
             base.Update();
        }

        public virtual void Remove()
        {
            if (Parent != null)
            {
                Parent.Children.Remove(this);
            }
        }

        public void RemoveChild(IEntity entity)
        {
            var itemToRemove = Children.SingleOrDefault(treeViewItemViewModel => treeViewItemViewModel.Entity.Id == entity.Id);
            if (itemToRemove != null)
            {
                Children.Remove(itemToRemove);
            }
        }

        private void EntityMoved(MoveEntityEventArgs moveEntityEventArgs)
        {
            //Check that type is valid
            if (Entity.GetType() != moveEntityEventArgs.NewParent.GetType())
            {
                return;
            }

            //Do nothing in case of a timeentry (since we dont have timeentries in the tree)
            if (moveEntityEventArgs.MovedEntity is TimeEntry)
            {
                return;
            }

            if (moveEntityEventArgs.OldParent.Id == Entity.Id)
            {
                RemoveChild(moveEntityEventArgs.MovedEntity);
            }

            if (moveEntityEventArgs.NewParent.Id == Entity.Id)
            {
                if (Children.SingleOrDefault(item => item.Entity.Id == moveEntityEventArgs.MovedEntity.Id) == null)
                {
                    AddChild(moveEntityEventArgs.MovedEntity);
                }
            }
        }

        public virtual void AddChild(IEntity entity) {}

        protected void OnLoadChildrenCompleted()
        {
            if (LoadChildrenCompleted != null)
            {
                LoadChildrenCompleted(this, null);
            }
        }
    }
}