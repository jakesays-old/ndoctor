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

namespace Probel.NDoctor.Domain.DTO.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Objects;

    public class PracticeRefiner : MemoryRefinerWithNameProperty<PracticeDto>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PracticeRefiner"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public PracticeRefiner(IEnumerable<PracticeDto> items)
            : base(items)
        {
        }

        #endregion Constructors
    }
}