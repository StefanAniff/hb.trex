using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TrexSL.Shell.Menu.MenuModel
{
    public class MenuItemsCollection : ObservableCollection<MenuItem>
    {
        public MenuItemsCollection()
            : this(null)
        {
            CollectionChanged += MenuItemsCollection_CollectionChanged;
        }

        public MenuItemsCollection(MenuItem parent)
        {
            Parent = parent;
        }

        public MenuItem Parent { get; set; }

        protected override void InsertItem(int index, MenuItem item)
        {
            item.Parent = Parent;

            base.InsertItem(index, item);
        }

        private void MenuItemsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {}
    }
}