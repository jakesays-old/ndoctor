﻿/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.Plugins.Authorisation.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Authorisation.Helpers;
    using Probel.NDoctor.Plugins.Authorisation.Properties;
    using Probel.NDoctor.Plugins.Authorisation.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    /// <summary>
    /// Workbench's ViewModel of the plugin
    /// </summary>
    public class WorkbenchViewModel : BaseViewModel
    {
        #region Fields

        private IAuthorisationComponent component;
        private TaskDto listViewTask;
        private RoleDto selectedRole;
        private TaskDto selectedTask;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkbenchViewModel"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WorkbenchViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
            }
            this.Roles = new ObservableCollection<RoleDto>();
            this.AvailableTasks = new ObservableCollection<TaskDto>();

            this.AddCommand = new RelayCommand(() => this.AddTask(), () => this.CanModifyTask());
            this.RemoveTaskCommand = new RelayCommand(() => this.RemoveTask(), () => this.CanModifyTask());
            this.RefreshRoleCommand = new RelayCommand(() => this.RefreshAvailableTasks());
            this.EditRoleCommand = new RelayCommand(() => this.EditRole(), () => this.CanEditRole());
            this.RemoveRoleCommand = new RelayCommand(() => this.RemoveRole(), () => this.CanRemoveRole());

            Notifyer.RoleRefreshing += (sender, e) => this.Refresh();
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TaskDto> AvailableTasks
        {
            get;
            private set;
        }

        public TaskDto ComboBoxTask
        {
            get { return this.selectedTask; }
            set
            {
                this.selectedTask = value;
                this.OnPropertyChanged(() => ComboBoxTask);
            }
        }

        public ICommand EditRoleCommand
        {
            get;
            private set;
        }

        public TaskDto ListViewTask
        {
            get { return this.listViewTask; }
            set
            {
                this.listViewTask = value;
                this.OnPropertyChanged(() => ListViewTask);
            }
        }

        public ICommand RefreshRoleCommand
        {
            get;
            private set;
        }

        public ICommand RemoveRoleCommand
        {
            get;
            private set;
        }

        public ICommand RemoveTaskCommand
        {
            get;
            private set;
        }

        public ObservableCollection<RoleDto> Roles
        {
            get;
            private set;
        }

        public RoleDto SelectedRole
        {
            get { return this.selectedRole; }
            set
            {
                this.selectedRole = value;
                this.OnPropertyChanged(() => SelectedRole);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Refreshes the whole data of this instance.
        /// </summary>
        public void Refresh()
        {
            try
            {
                RoleDto[] result;
                using (this.component.UnitOfWork)
                {
                    result = this.component.GetAllRoles();
                }
                this.Roles.Refill(result);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void AddTask()
        {
            try
            {
                this.SelectedRole.Tasks.Add(this.ComboBoxTask);
                this.RefreshAvailableTasks();

                this.UpdateRoles();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_TaskCreated);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private bool CanEditRole()
        {
            return true;
        }

        private bool CanModifyTask()
        {
            return this.SelectedRole != null;
        }

        private bool CanRemoveRole()
        {
            return this.SelectedRole != null;
        }

        private bool CanRemoveRole(RoleDto roleDto)
        {
            var result = false;
            using (component.UnitOfWork)
            {
                result = component.CanRemove(roleDto);
            }
            return result;
        }

        private void EditRole()
        {
            InnerWindow.Show(Messages.Menu_Edit, new EditRoleView(this.SelectedRole));
        }

        private void RefreshAvailableTasks()
        {
            try
            {
                TaskDto[] result;
                using (this.component.UnitOfWork)
                {
                    result = this.component.GetAvailableTasks(this.SelectedRole);
                }
                this.AvailableTasks.Refill(result);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void RemoveRole()
        {
            if (CanRemoveRole(this.SelectedRole))
            {
                var result = MessageBox.Show(Messages.Question_DeleteRole, BaseText.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                using (component.UnitOfWork)
                {
                    component.Remove(this.SelectedRole);
                }
                this.Refresh();
            }
            else
            {
                MessageBox.Show(Messages.Msg_CantRemoveRole, BaseText.Question, MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void RemoveTask()
        {
            try
            {
                this.SelectedRole.Tasks.Remove(this.ListViewTask);
                this.UpdateRoles();
                this.RefreshAvailableTasks();
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_TaskRemoved);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void UpdateRoles()
        {
            try
            {
                using (this.component.UnitOfWork)
                {
                    this.component.Update(this.SelectedRole);
                }
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Methods
    }
}