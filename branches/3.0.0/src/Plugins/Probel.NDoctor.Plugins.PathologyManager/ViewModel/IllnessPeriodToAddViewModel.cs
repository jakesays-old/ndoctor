﻿namespace Probel.NDoctor.Plugins.PathologyManager.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PathologyManager.Helpers;
    using Probel.NDoctor.Plugins.PathologyManager.Properties;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class IllnessPeriodToAddViewModel : IllnessPeriodDto
    {
        #region Fields

        private IPathologyComponent component = ComponentFactory.PathologyComponent;
        private ErrorHandler errorHandler = null;
        private bool isSelected;

        #endregion Fields

        #region Constructors

        public IllnessPeriodToAddViewModel()
        {
            this.errorHandler = new ErrorHandler(this);
            this.Start
                = this.End
                = DateTime.Today;
            this.AddCommand = new RelayCommand(() => this.Add(), () => this.CanAdd());
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public string BtnAdd
        {
            get { return Messages.Title_Add; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected in the ListView.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public string TitleIllFrom
        {
            get { return Messages.Title_IllFrom; }
        }

        public string TitleTo
        {
            get { return Messages.Title_To; }
        }

        #endregion Properties

        #region Methods

        private void Add()
        {
            Assert.IsNotNull(PluginContext.Host, "Host");

            try
            {
                var dr = MessageBox.Show(Messages.Msg_AskAddNewPeriod, Messages.Title_Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.No) return;

                using (this.component.UnitOfWork)
                {
                    this.component.Create(this, PluginContext.Host.SelectedPatient);
                }
                PluginContext.Host.WriteStatus(StatusType.Info, Messages.Msg_PeriodAdded);
                Notifyer.OnItemChanged(this);
            }
            catch (Exception ex)
            {
                this.errorHandler.HandleError(ex, Messages.Msg_FailAddIllnessPeriod);
            }
        }

        private bool CanAdd()
        {
            return (this.End > this.Start);
        }

        #endregion Methods
    }
}