﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Probel.NDoctor.Domain.DAL.Properties {
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
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Probel.NDoctor.Domain.DAL.Properties.Messages", typeof(Messages).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to More than one default user was found..
        /// </summary>
        internal static string Ex_QueryException_SeveralDefaultUsers {
            get {
                return ResourceManager.GetString("Ex_QueryException_SeveralDefaultUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t remove this item because it is referenced somewhere else..
        /// </summary>
        internal static string Ex_ReferencialIntegrityException_Deletion {
            get {
                return ResourceManager.GetString("Ex_ReferencialIntegrityException_Deletion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with this right can modify data about administration.
        /// </summary>
        internal static string Explanation_Administer {
            get {
                return ResourceManager.GetString("Explanation_Administer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The administrator has all the rights in the application.
        /// </summary>
        internal static string Explanation_Administrator {
            get {
                return ResourceManager.GetString("Explanation_Administrator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The doctor can read and mofify all the data but he/she cant read or modify  adlinistration data.
        /// </summary>
        internal static string Explanation_Doctor {
            get {
                return ResourceManager.GetString("Explanation_Doctor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Un utilisateur qui a ce droit peut ajouter, modifier ou supprimer des rendez-vous dans l&apos;agenda..
        /// </summary>
        internal static string Explanation_EditCalendar {
            get {
                return ResourceManager.GetString("Explanation_EditCalendar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Everyone can execute this feature.
        /// </summary>
        internal static string Explanation_Everyone {
            get {
                return ResourceManager.GetString("Explanation_Everyone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with this right can modify data about himself/herself such as his/her password.
        /// </summary>
        internal static string Explanation_Metawrite {
            get {
                return ResourceManager.GetString("Explanation_Metawrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with this right can read data.
        /// </summary>
        internal static string Explanation_Read {
            get {
                return ResourceManager.GetString("Explanation_Read", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The secretary can read all the data but only modify the calendar.
        /// </summary>
        internal static string Explanation_Secretary {
            get {
                return ResourceManager.GetString("Explanation_Secretary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with this right can write and edit data (but not about himself/herself or administration).
        /// </summary>
        internal static string Explanation_Write {
            get {
                return ResourceManager.GetString("Explanation_Write", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Impossible to create the database.
        /// </summary>
        internal static string Msg_ErrorDbCreationImpossible {
            get {
                return ResourceManager.GetString("Msg_ErrorDbCreationImpossible", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to the database is invalid or the file doesn&apos;t exist.
        /// </summary>
        internal static string Msg_ErrorDbInvalidPath {
            get {
                return ResourceManager.GetString("Msg_ErrorDbInvalidPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path where to export the HBM files is invalid or doesn&apos;t exist!.
        /// </summary>
        internal static string Msg_ErrorInvalidHbmPath {
            get {
                return ResourceManager.GetString("Msg_ErrorInvalidHbmPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A session is already opened. Please, close the opened session before opening a new one..
        /// </summary>
        internal static string Msg_ErrorSessionAlreadyOpenException {
            get {
                return ResourceManager.GetString("Msg_ErrorSessionAlreadyOpenException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The session factory is not set. Are you sure you have configured nHibernate for this instance?.
        /// </summary>
        internal static string Msg_ErrorSessionFactoryNull {
            get {
                return ResourceManager.GetString("Msg_ErrorSessionFactoryNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The session is not open. Please open it before querying the database..
        /// </summary>
        internal static string Msg_ErrorSessionNotOpen {
            get {
                return ResourceManager.GetString("Msg_ErrorSessionNotOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SQLiteConfiguration is not set!.
        /// </summary>
        internal static string Msg_ErrorSQLiteConfigurationNotSet {
            get {
                return ResourceManager.GetString("Msg_ErrorSQLiteConfigurationNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Administrator.
        /// </summary>
        internal static string Role_Administrator {
            get {
                return ResourceManager.GetString("Role_Administrator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Doctor.
        /// </summary>
        internal static string Role_Doctor {
            get {
                return ResourceManager.GetString("Role_Doctor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Secretary.
        /// </summary>
        internal static string Role_Secretary {
            get {
                return ResourceManager.GetString("Role_Secretary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Administer.
        /// </summary>
        internal static string Task_Administer {
            get {
                return ResourceManager.GetString("Task_Administer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Editer les agendas.
        /// </summary>
        internal static string Task_EditCalendar {
            get {
                return ResourceManager.GetString("Task_EditCalendar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Everyone.
        /// </summary>
        internal static string Task_Everyone {
            get {
                return ResourceManager.GetString("Task_Everyone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Metawrite.
        /// </summary>
        internal static string Task_Metawrite {
            get {
                return ResourceManager.GetString("Task_Metawrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Read.
        /// </summary>
        internal static string Task_Read {
            get {
                return ResourceManager.GetString("Task_Read", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Write.
        /// </summary>
        internal static string Task_Write {
            get {
                return ResourceManager.GetString("Task_Write", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You didn&apos;t entered twice the same password. .
        /// </summary>
        internal static string Validation_Password {
            get {
                return ResourceManager.GetString("Validation_Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password can&apos;t be empty.
        /// </summary>
        internal static string Validation_PasswordCantBeEmpty {
            get {
                return ResourceManager.GetString("Validation_PasswordCantBeEmpty", resourceCulture);
            }
        }
    }
}
