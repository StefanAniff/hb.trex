﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trex.Server.Core.Resources {
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
    public class UserManagementResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserManagementResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Trex.Server.Core.Resources.UserManagementResource", typeof(UserManagementResource).Assembly);
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
        ///   Looks up a localized string similar to The user were successfully created.
        /// </summary>
        public static string CreationSuccess {
            get {
                return ResourceManager.GetString("CreationSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The email is already used by another user in the system.
        /// </summary>
        public static string EmailAlreadyExistsText {
            get {
                return ResourceManager.GetString("EmailAlreadyExistsText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password provided is not in compliance with the password rules set up by the system.
        /// </summary>
        public static string InvalidPasswordText {
            get {
                return ResourceManager.GetString("InvalidPasswordText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unspecified error occured.
        /// </summary>
        public static string UnspecifiedError {
            get {
                return ResourceManager.GetString("UnspecifiedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user already exists.
        /// </summary>
        public static string UserAlreadyExistText {
            get {
                return ResourceManager.GetString("UserAlreadyExistText", resourceCulture);
            }
        }
    }
}
