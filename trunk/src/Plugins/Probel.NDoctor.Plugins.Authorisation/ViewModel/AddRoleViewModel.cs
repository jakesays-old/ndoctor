﻿#region Header

/*
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

#endregion Header

namespace Probel.NDoctor.Plugins.Authorisation.ViewModel
{
    using System;
    using System.Windows.Input;

    using Probel.Helpers.WPF;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.Authorisation.Helpers;
    using Probel.NDoctor.Plugins.Authorisation.Properties;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class AddRoleViewModel : BaseViewModel
    {
        #region Fields

        private IAuthorisationComponent component;
        private string roleName;

        #endregion Fields

        #region Constructors

        public AddRoleViewModel()
        {
            if (!Designer.IsDesignMode)
            {
                this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
                PluginContext.Host.NewUserConnected += (sender, e) => this.component = PluginContext.ComponentFactory.GetInstance<IAuthorisationComponent>();
            }
            this.AddCommand = new RelayCommand(() => this.AddRole(), () => this.CanAddRole());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string RoleName
        {
            get { return this.roleName; }
            set
            {
                this.roleName = value;
                this.OnPropertyChanged(() => RoleName);
            }
        }

        #endregion Properties

        #region Methods

        private void AddRole()
        {
            try
            {
                using (component.UnitOfWork)
                {
                    this.component.Create(new RoleDto()
                    {
                        Name = this.RoleName
                    });
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_RoleCreated);
                Notifyer.OnRoleRefreshing(this);
                InnerWindow.Close();
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private bool CanAddRole()
        {
            return !string.IsNullOrWhiteSpace(this.RoleName);
        }

        #endregion Methods
    }
}