using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Repository;
using HallData.Admin.ApplicationViews;
using System.Threading;

namespace HallData.Admin.Data
{
    public interface IReadOnlyInterfaceAttributeRepository : IReadOnlyRepository<InterfaceAttributeResult>
    {
        Task<QueryResults<InterfaceAttributeResult>> GetByInterface(int interfaceId, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IInterfaceAttributeRepository : IDeletableRepository<int, InterfaceAttributeResult, InterfaceAttributeForAdd, InterfaceAttributeForUpdate>, IReadOnlyInterfaceAttributeRepository
    {
    }
    
}
