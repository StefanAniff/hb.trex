using System.Windows;
using System.Windows.Controls;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Infrastructure.Implemented;

namespace Trex.SmartClient.Infrastructure.AttachedProperties
{
    public class PermissionControl : FrameworkElement
    {




        /// <summary>
        /// The RequiredEnabledPermission attached property's name.
        /// </summary>
        public const string RequiredEnabledPermissionPropertyName = "RequiredEnabledPermission";

        /// <summary>
        /// Gets the value of the RequiredEnabledPermission attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the RequiredEnabledPermission property of the specified object.</returns>
        public static Permissions GetRequiredEnabledPermission(UIElement obj)
        {
            return (Permissions)obj.GetValue(RequiredEnabledPermissionProperty);
        }

        /// <summary>
        /// Sets the value of the RequiredEnabledPermission attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the RequiredEnabledPermission value of the specified object.</param>
        public static void SetRequiredEnabledPermission(UIElement obj, Permissions value)
        {
            obj.SetValue(RequiredEnabledPermissionProperty, value);
        }

        /// <summary>
        /// Identifies the RequiredEnabledPermission attached property.
        /// </summary>
        public static readonly DependencyProperty RequiredEnabledPermissionProperty = DependencyProperty.RegisterAttached(
            RequiredEnabledPermissionPropertyName,
            typeof(Permissions),
            typeof(Control),
            new FrameworkPropertyMetadata(OnRequiredEnabledPermissionChanged));

        private static void OnRequiredEnabledPermissionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            ((Control) d).IsEnabled = IsAllowed((Permissions) e.NewValue);

        }


        /// <summary>
        /// The AllowedRoles attached property's name.
        /// </summary>
        public const string RequiredVisibilityPermissionPropertyName = "RequiredVisibilityPermission";

        /// <summary>
        /// Gets the value of the AllowedRoles attached property 
        /// for a given dependency object.
        /// </summary>
        /// <param name="obj">The object for which the property value
        /// is read.</param>
        /// <returns>The value of the AllowedRoles property of the specified object.</returns>
        public static Permissions GetRequiredVisibilityPermission(UIElement obj)
        {
            return (Permissions)obj.GetValue(RequiredVisibilityPermissionProperty);
        }

        /// <summary>
        /// Sets the value of the AllowedRoles attached property
        /// for a given dependency object. 
        /// </summary>
        /// <param name="obj">The object to which the property value
        /// is written.</param>
        /// <param name="value">Sets the AllowedRoles value of the specified object.</param>
        public static void SetRequiredVisibilityPermission(UIElement obj, Permissions value)
        {
            obj.SetValue(RequiredVisibilityPermissionProperty, value);
        }

        /// <summary>
        /// Identifies the AllowedRoles attached property.
        /// </summary>
        public static readonly DependencyProperty RequiredVisibilityPermissionProperty = DependencyProperty.RegisterAttached(
            RequiredVisibilityPermissionPropertyName,
            typeof(Permissions),
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(OnRequiredVisibilityPermissionChanged));

        private static void OnRequiredVisibilityPermissionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            ((FrameworkElement) d).Visibility = IsAllowed((Permissions)e.NewValue)
                                                    ? Visibility.Visible
                                                    : Visibility.Collapsed;


        }

        public static bool IsAllowed(Permissions requiredPermission)
        {
            return UserContext.Instance.User.HasPermission(requiredPermission);


        }
    }
}
