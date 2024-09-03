using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.Data
{
    public interface IReadOnlyCustomerPersonRepository : IReadOnlyPersonRepository<CustomerId, CustomerPersonResult> { }
    public interface ICustomerPersonRepository : IPersonRepository<CustomerId, CustomerPersonResult, CustomerPersonForAdd, CustomerPersonForUpdate>, IReadOnlyCustomerPersonRepository { }
}
