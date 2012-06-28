﻿namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class AddDoctorViewModel : BaseBoxViewModel<DoctorDto>
    {
        #region Constructors

        public AddDoctorViewModel()
        {
            this.Specialisations = new ObservableCollection<TagDto>();
            this.BoxItem = new DoctorDto();

            try
            {
                IList<TagDto> result = new List<TagDto>();
                using (this.Component.UnitOfWork)
                {
                    result = this.Component.FindTags(TagCategory.Doctor);
                }
                this.Specialisations.Refill(result);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<TagDto> Specialisations
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        protected override void AddItem()
        {
            using (this.Component.UnitOfWork)
            {
                this.Component.Create(this.BoxItem);
            }
        }

        #endregion Methods
    }
}