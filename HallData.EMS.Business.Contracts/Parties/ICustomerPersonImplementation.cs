using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Business;


namespace HallData.EMS.Business
{
    public interface IReadOnlyCustomerPersonImplementation : IReadOnlyPersonImplementation<CustomerId, CustomerPersonResult>, IReadOnlyCustomerImplementation<CustomerPersonResult> { }
    public interface ICustomerPersonImplementationBase : ICustomerImplementationBase<CustomerPersonResult> { }
    [Service("customers/people")]
    public interface ICustomerPersonImplementation : IPersonImplementation<CustomerId, CustomerPersonResult, CustomerPersonForAdd, CustomerPersonForUpdate>, 
        IReadOnlyCustomerPersonImplementation, ICustomerPersonImplementationBase, 
        ICustomerImplementation<CustomerPersonResult, CustomerPersonForAdd, CustomerPersonForUpdate> { }
}
