﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GmodAddonManager.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GmodAddonManager.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Bitmap add {
            get {
                object obj = ResourceManager.GetObject("add", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to url.
        /// </summary>
        internal static string addRepoDefaultValue {
            get {
                return ResourceManager.GetString("addRepoDefaultValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add....
        /// </summary>
        internal static string addRepoHeader {
            get {
                return ResourceManager.GetString("addRepoHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please give the url to the repository
        ///git support disabled.
        /// </summary>
        internal static string addRepoMessageGitDisabled {
            get {
                return ResourceManager.GetString("addRepoMessageGitDisabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please give the url to the repository
        ///git support enabled.
        /// </summary>
        internal static string addRepoMessageGitEnabled {
            get {
                return ResourceManager.GetString("addRepoMessageGitEnabled", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap arrow_refresh {
            get {
                object obj = ResourceManager.GetObject("arrow_refresh", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap delete {
            get {
                object obj = ResourceManager.GetObject("delete", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Edit error.
        /// </summary>
        internal static string editErrorHeader {
            get {
                return ResourceManager.GetString("editErrorHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only edit one addon at a time.
        /// </summary>
        internal static string editTooManySelected {
            get {
                return ResourceManager.GetString("editTooManySelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No adddons dir found.
        ///Make sure you have installed and launched Garry&apos;s mod at least once
        ///
        ///Program will not exit.
        /// </summary>
        internal static string noAddonsDirFoundError {
            get {
                return ResourceManager.GetString("noAddonsDirFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ADDON DIR NOT SET
        ///PLEASE SET IT NOW.
        /// </summary>
        internal static string notSetString {
            get {
                return ResourceManager.GetString("notSetString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder name....
        /// </summary>
        internal static string repoFolderHeader {
            get {
                return ResourceManager.GetString("repoFolderHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please give name of the folder where to save
        ///THIS MUST BE SET IF NOT TOLD OTHERWISE
        ///Usually the name of the mod.
        /// </summary>
        internal static string repoFolderMessage {
            get {
                return ResourceManager.GetString("repoFolderMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Update.
        /// </summary>
        internal static string updateButDefaultText {
            get {
                return ResourceManager.GetString("updateButDefaultText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Updating....
        /// </summary>
        internal static string updateButtonUpdating {
            get {
                return ResourceManager.GetString("updateButtonUpdating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Update complete.
        /// </summary>
        internal static string updateCompleteHeader {
            get {
                return ResourceManager.GetString("updateCompleteHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Done updating addons.
        /// </summary>
        internal static string updateCompleteMessage {
            get {
                return ResourceManager.GetString("updateCompleteMessage", resourceCulture);
            }
        }
    }
}
