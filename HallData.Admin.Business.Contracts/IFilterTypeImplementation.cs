using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Business;
using System.Threading;

namespace HallData.Admin.Business
{
    public interface IReadOnlyFilterTypeImplementation : IReadOnlyBusinessImplementation<FilterTypeResult>
    {
    }
    [Service("filtertypes")]
    public interface IFilterTypeImplementation : IDeletableBusinessImplementationWithBase<int, FilterTypeResult, FilterTypeForAdd, FilterTypeForUpdate>, IReadOnlyFilterTypeImplementation
    {
    }
}
