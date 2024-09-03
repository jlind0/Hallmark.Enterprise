using System;
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
    public interface IReadOnlyInterfaceRepository : IReadOnlyRepository<InterfaceResult>
    {
        Task<QueryResult<InterfaceResult>> GetByName(string name, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task<bool> AreInterfacesCommon(int interfaceId1, int interfaceId2, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface IInterfaceRepository : IDeletableRepository<int, InterfaceResult, InterfaceForAdd, InterfaceForUpdate>, IReadOnlyInterfaceRepository
    {
        Task RelateInterfaces(int interfaceId, int relatedInterfaceId, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task UnRelateInterfaces(int interfaceId, int relatedInterfaceId, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}
