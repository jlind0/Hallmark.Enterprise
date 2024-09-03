﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Repository;
using System.Threading;
using HallData.ApplicationViews;

namespace HallData.Admin.Data
{
    public interface IReadOnlyFilterTypeRepository : IReadOnlyRepository<FilterTypeResult>
    {
    }
    public interface IFilterTypeRepository : IDeletableRepository<int, FilterTypeResult, FilterTypeForAdd, FilterTypeForUpdate>, IReadOnlyFilterTypeRepository
    {

    }
}
