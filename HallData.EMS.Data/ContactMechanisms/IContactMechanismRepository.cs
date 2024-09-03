using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface IContactMechanismRepository : IDeletableRepository<Guid, ContactMechanismGeneric, ContactMechanismGenericForAdd, ContactMechanismGenericForUpdate>
    {
        Task<QueryResults<ContactMechanismGeneric>> Get(MechanismTypes mechanismType, string viewName = null, Guid? userId = null, FilterContext<ContactMechanismGeneric> filter = null, SortContext<ContactMechanismGeneric> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<JObject>> GetView(MechanismTypes mechanismType, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
}
