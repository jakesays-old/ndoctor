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
namespace Probel.NDoctor.Plugins.PatientData
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PatientData.Properties;
    using Probel.NDoctor.Plugins.PatientData.View;
    using Probel.NDoctor.Plugins.PatientData.ViewModel;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;
    using Probel.NDoctor.View.Toolbox.Navigation;

    [Export(typeof(IPlugin))]
    public class PatientData : StaticViewPlugin<WorkbenchView>
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.PatientData;component/Images\{0}.png";

        private ICommand addDoctorCommand;
        private ICommand addInsuranceCommand;
        private ICommand addPracticeCommand;
        private ICommand addProfessionCommand;
        private ICommand addReputationCommand;
        private ICommand addSpecialisationCommand;
        private IPatientDataComponent component;
        private ICommand navigateCommand;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public PatientData([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);
        }

        #endregion Constructors

        #region Properties

        public ICommand BindDoctorCommand
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public override void Initialise()
        {
            this.component = PluginContext.ComponentFactory.GetInstance<IPatientDataComponent>();
            PluginContext.Host.Invoke(() =>
            {
                this.ConfigureViewService();
                this.BuildButtons();
                this.BuildContextMenu();
            });
        }

        private void BuildButtons()
        {
            this.navigateCommand = new RelayCommand(() => this.Navigate(), () => this.CanNavigate());

            this.addDoctorCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<CreateDoctorViewModel>(), () => this.IsGrantedToWrite());
            this.addSpecialisationCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddSpecialisationViewModel>(), () => this.IsGrantedToWrite());
            this.addInsuranceCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddInsuranceViewModel>(), () => this.IsGrantedToWrite());
            this.addReputationCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddReputationViewModel>(), () => this.IsGrantedToWrite());
            this.addPracticeCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddPracticeViewModel>(), () => this.IsGrantedToWrite());
            this.addProfessionCommand = new RelayCommand(() => ViewService.Manager.ShowDialog<AddProfessionViewModel>(), () => this.IsGrantedToWrite());

            var navigateButton = new RibbonButtonData(Messages.Title_PatientDataManager
                    , imgUri.FormatWith("Properties")
                    , navigateCommand) { Order = 1 };

            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
        }

        private void BuildContextMenu()
        {
            var saveButton = new RibbonButtonData(Messages.Title_Save, imgUri.FormatWith("Save"), this.View.As<WorkbenchViewModel>().SaveCommand);

            var rollbackButton = new RibbonButtonData(Messages.Title_Rollback, imgUri.FormatWith("Save"), this.View.As<WorkbenchViewModel>().RollbackCommand);

            var splitter = new RibbonMenuItemData(Messages.Btn_Add, imgUri.FormatWith("Add"), null);
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddDoctor, imgUri.FormatWith("Add"), this.addDoctorCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddSpecialisation, imgUri.FormatWith("Add"), this.addSpecialisationCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddInsurance, imgUri.FormatWith("Add"), this.addInsuranceCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddReputation, imgUri.FormatWith("Add"), this.addReputationCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddPractice, imgUri.FormatWith("Add"), this.addPracticeCommand));
            splitter.ControlDataCollection.Add(new RibbonMenuItemData(Messages.Title_AddProfession, imgUri.FormatWith("Add"), this.addProfessionCommand));

            var cgroup = new RibbonGroupData(Messages.Menu_Actions);
            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(rollbackButton);
            cgroup.ButtonDataCollection.Add(splitter);

            var tab = new RibbonTabData(Messages.Menu_File) { ContextualTabGroupHeader = Messages.Title_ContextMenu };
            tab.GroupDataCollection.Add(cgroup);
            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_ContextMenu, tab) { Background = Brushes.OrangeRed, IsVisible = false };
            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private bool CanNavigate()
        {
            return PluginContext.Host.SelectedPatient != null
                && PluginContext.DoorKeeper.IsUserGranted(To.Read);
        }

        private void ConfigureViewService()
        {
            ViewService.Configure(e =>
            {
                e.Bind<CreateDoctorView, CreateDoctorViewModel>()
                 .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
                e.Bind<AddSpecialisationView, AddSpecialisationViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
                e.Bind<AddInsuranceView, AddInsuranceViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
                e.Bind<AddReputationView, AddReputationViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
                e.Bind<AddPracticeView, AddPracticeViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
                e.Bind<AddProfessionView, AddProfessionViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
                e.Bind<BindDoctorView, BindDoctorViewModel>()
                    .OnClosing(() => this.View.As<WorkbenchViewModel>().Refresh());
            });
        }

        private bool IsGrantedToWrite()
        {
            return PluginContext.DoorKeeper.IsUserGranted(To.Write);
        }

        private void Navigate()
        {
            this.View.As<WorkbenchViewModel>().Refresh();
            PluginContext.Host.Navigate(this.View);
            this.ShowContextMenu();
        }

        #endregion Methods
    }
}