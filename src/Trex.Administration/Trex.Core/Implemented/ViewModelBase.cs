using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using Trex.Core.Interfaces;

namespace Trex.Core.Implemented
{
    public abstract class ViewModelBase :IViewModel, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void Validate(string memberName, object value)
        {
            var validationContext = new ValidationContext(this, null, null);
            validationContext.MemberName = memberName;
            Validator.ValidateProperty(value, validationContext);
        }

        public void Update()
        {
            var props = GetType().GetProperties();

            foreach (var propertyInfo in props)
            {
                if (propertyInfo.MemberType == MemberTypes.Property || propertyInfo.MemberType == MemberTypes.NestedType)
                {
                    OnPropertyChanged(propertyInfo.Name);
                }
            }
        }

        public virtual void Close()
        {
            
        }
    }
}