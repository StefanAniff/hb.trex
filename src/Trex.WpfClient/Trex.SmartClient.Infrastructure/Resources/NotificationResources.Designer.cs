﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trex.SmartClient.Infrastructure.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class NotificationResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal NotificationResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Trex.SmartClient.Infrastructure.Resources.NotificationResources", typeof(NotificationResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to hours.
        /// </summary>
        public static string HourText {
            get {
                return ResourceManager.GetString("HourText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Text;
        ///using Trex.SmartClient.Core.Interfaces;
        ///
        ///namespace Trex.SmartClient.TaskModule.Interfaces
        ///{
        ///    public interface ISettingsViewModel:IViewModel
        ///    {
        ///    }
        ///}
        ///.
        /// </summary>
        public static string ISettingsViewModel {
            get {
                return ResourceManager.GetString("ISettingsViewModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The T.Rex SmartClient is running.
        /// </summary>
        public static string SystemActiveText {
            get {
                return ResourceManager.GetString("SystemActiveText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Still working?.
        /// </summary>
        public static string SystemActiveTitle {
            get {
                return ResourceManager.GetString("SystemActiveTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The T.Rex SmartClient has been idle for.
        /// </summary>
        public static string SystemIdleText {
            get {
                return ResourceManager.GetString("SystemIdleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you working on a project?.
        /// </summary>
        public static string SystemIdleTitle {
            get {
                return ResourceManager.GetString("SystemIdleTitle", resourceCulture);
            }
        }
    }
}
