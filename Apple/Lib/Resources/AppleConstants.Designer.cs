﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.17020
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class AppleConstants {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppleConstants() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("com.interactiverobert.prototypes.movieexplorer.apple.lib.Resources.AppleConstants", typeof(AppleConstants).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string FavoriteListChanged_NotificationName {
            get {
                return ResourceManager.GetString("FavoriteListChanged_NotificationName", resourceCulture);
            }
        }
        
        public static string ShowMovieDetail_SegueName {
            get {
                return ResourceManager.GetString("ShowMovieDetail_SegueName", resourceCulture);
            }
        }
        
        public static string ShowMovieList_SegueName {
            get {
                return ResourceManager.GetString("ShowMovieList_SegueName", resourceCulture);
            }
        }
        
        public static string MovieCollectionViewCell_ReuseKey {
            get {
                return ResourceManager.GetString("MovieCollectionViewCell_ReuseKey", resourceCulture);
            }
        }
        
        public static string MovieCategoryTableViewCell_ReuseKey {
            get {
                return ResourceManager.GetString("MovieCategoryTableViewCell_ReuseKey", resourceCulture);
            }
        }
    }
}
