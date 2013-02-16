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

namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DAL.Subcomponents;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Exposes tools to find and fix incoherences in the data.
    /// </summary>
    public class RescueToolsComponent : BaseComponent, IRescueToolsComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RescueToolsComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public RescueToolsComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RescueToolsComponent"/> class.
        /// </summary>
        public RescueToolsComponent()
            : base()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Activates the specified deactivated patients.
        /// </summary>
        /// <param name="patients">The patients.</param>
        public void Activate(IEnumerable<LightPatientDto> patients)
        {
            foreach (var patient in patients)
            {
                var entity = this.Session.Get<Patient>(patient.Id);
                entity.IsDeactivated = false;
                this.Session.Update(entity);
            }
        }

        /// <summary>
        /// Deactivates the specified patients.
        /// </summary>
        /// <param name="patients">The patients.</param>
        public void Deactivate(IEnumerable<LightPatientDto> patients)
        {
            foreach (var patient in patients)
            {
                var entity = this.Session.Get<Patient>(patient.Id);
                entity.IsDeactivated = true;
                this.Session.Update(entity);
            }
        }

        /// <summary>
        /// Finds the deactivated patients.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LightPatientDto> GetDeactivated()
        {
            var results = (from p in this.Session.Query<Patient>()
                           where p.IsDeactivated == true
                           select p);
            return Mapper.Map<IEnumerable<Patient>, IEnumerable<LightPatientDto>>(results);
        }

        /// <summary>
        /// Gets a list of the doctor doubloons.
        /// </summary>
        /// <returns>
        /// A list of doubloons
        /// </returns>
        public IEnumerable<DoubloonDoctorDto> GetDoctorDoubloons()
        {
            var doubloons = (from doc in this.Session.Query<Doctor>().ToList()
                             group doc by new { doc.FirstName, doc.LastName, doc.Specialisation } into g
                             where g.Count() > 1
                             select new { g.Key, Count = g.Count() })
                             .OrderByDescending(e => e.Count)
                             .AsEnumerable();

            List<DoubloonDoctorDto> result = new List<DoubloonDoctorDto>();
            foreach (var doubloon in doubloons)
            {
                result.Add(new DoubloonDoctorDto()
                {
                    Count = doubloon.Count,
                    FirstName = doubloon.Key.FirstName,
                    LastName = doubloon.Key.LastName,
                    Specialisation = Mapper.Map<Tag, TagDto>(doubloon.Key.Specialisation),
                });
            }
            return result;
        }

        /// <summary>
        /// Gets the doubloons of the specified doctor.
        /// </summary>
        /// <param name="doctor">The doctor that will be useds to find doubloons.</param>
        /// <returns>
        /// An enumeration of doctor that are doubloons with the specified doctor
        /// </returns>
        public IEnumerable<LightDoctorDto> GetDoubloonsOf(LightDoctorDto doctor)
        {
            var entities = (from doc in this.Session.Query<Doctor>()
                            where doc.FirstName == doctor.FirstName
                               && doc.LastName == doctor.LastName
                               && doc.Specialisation.Id == doctor.Specialisation.Id
                               && doc.Id != doctor.Id
                            select doc).AsEnumerable();
            return Mapper.Map<IEnumerable<Doctor>, IEnumerable<LightDoctorDto>>(entities);
        }

        /// <summary>
        /// Gets the doubloons of the specified doctor.
        /// </summary>
        /// <param name="firstName">The first name of the doctor.</param>
        /// <param name="lastName">The last name of the doctor.</param>
        /// <param name="specialisation">The specialisation of the doctor.</param>
        /// <returns>
        /// An enumeration of doctor that are doubloons with the specified doctor
        /// </returns>
        public IEnumerable<LightDoctorDto> GetDoubloonsOf(string firstName, string lastName, TagDto specialisation)
        {
            var entities = (from doc in this.Session.Query<Doctor>()
                            where doc.FirstName == firstName
                               && doc.LastName == lastName
                               && doc.Specialisation.Id == specialisation.Id
                            select doc).AsEnumerable();
            return Mapper.Map<IEnumerable<Doctor>, IEnumerable<LightDoctorDto>>(entities);
        }

        /// <summary>
        /// Finds the patients older than the specified age.
        /// </summary>
        /// <param name="age">The age of the patient in years.</param>
        /// <returns></returns>
        public IEnumerable<LightPatientDto> GetOlderThan(int age)
        {
            var date = DateTime.Today.AddYears(0 - age);

            var results = (from p in this.Session.Query<Patient>()
                           where p.BirthDate <= date
                              && p.IsDeactivated == false
                           select p);
            var dto = Mapper
                .Map<IEnumerable<Patient>, IEnumerable<LightPatientDto>>(results)
                .OrderByDescending(e => e.Age);
            return dto;
        }

        /// <summary>
        /// Replaces the specified doubloons with the specified doctor.
        /// </summary>
        /// <param name="doubloons">The doubloons.</param>
        /// <param name="withDoctor">The doctor that will replace the doubloons.</param>
        public void Replace(IEnumerable<LightDoctorDto> doubloons, LightDoctorDto withDoctor)
        {
            var updator = new Updator(this.Session);

            //Select the Id of doubloons and be sure the replacement is not in the list of items to remove
            var ids = (from d in doubloons
                       where d.Id != withDoctor.Id
                       select d.Id).ToList();

            // Find the replacement doctor
            var newDoctor = (from doc in this.Session.Query<Doctor>()
                             where doc.Id == withDoctor.Id
                             select doc).Single();

            //Find the patients that has on of the doubloons
            var patients = (from pat in this.Session.Query<Patient>().ToList()
                            where pat.Doctors.Where(e => ids.Contains(e.Id)).Count() > 0
                            select pat);
            // Replace the doubloons with the replacement patient
            foreach (var patient in patients)
            {
                //foreach (var doctor in patient.Doctors)
                for (int i = 0; i < patient.Doctors.Count; i++)
                {
                    var doctor = patient.Doctors[i];
                    if (ids.Contains(doctor.Id))
                    {
                        var toRemove = this.Session.Get<Doctor>(doctor.Id);
                        patient.Doctors.Remove(toRemove);
                        updator.AddDoctorTo(patient, newDoctor);
                        this.Session.Update(patient);
                    }
                }
            }
            // Remove old not used doctors
            foreach (var id in ids)
            {
                var doctor = this.Session.Get<Doctor>(id);
                if (doctor != null) { this.Session.Delete(doctor); }
            }
        }

        /// <summary>
        /// Replaces the with first doubloon.
        /// </summary>
        /// <param name="doubloons">The doubloons.</param>
        public void ReplaceWithFirstDoubloon(IEnumerable<LightDoctorDto> doubloons)
        {
            if (doubloons.Count() > 0)
            {
                foreach (var d in doubloons)
                {
                    var currentDoubloons = this.GetDoubloonsOf(d.FirstName, d.LastName, d.Specialisation);
                    if (currentDoubloons.Count() > 0)
                    {
                        this.Replace(currentDoubloons, currentDoubloons.First());
                    }
                    else { throw new NotSupportedException("There is no doubloons for the specified doctor"); }

                }
            }
            else { Logger.Debug("The list of doubloons was empty"); }
        }

        /// <summary>
        /// Updates the deactivation value for the specified patients.
        /// </summary>
        /// <param name="patients">The patients.</param>
        public void UpdateDeactivation(IEnumerable<LightPatientDto> patients)
        {
            foreach (var patient in patients)
            {
                var entity = this.Session.Get<Patient>(patient.Id);
                entity.IsDeactivated = patient.IsDeactivated;
                this.Session.Update(entity);
            }
        }

        #endregion Methods
    }
}