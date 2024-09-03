using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using System.Threading;
using HallData.Business;

namespace HallData.EMS.Business
{
	public interface IReadOnlyCustomerOrganizationImplementation : IReadOnlyOrganizationImplementation<CustomerId, CustomerOrganizationResult>, IReadOnlyCustomerImplementation<CustomerOrganizationResult> { }

	public interface ICustomerOrganizationImplementationBase : ICustomerImplementationBase<CustomerOrganizationResult> { }

	[Service("customers/organizations")]
	public interface ICustomerOrganizationImplementation : IOrganizationImplementation<CustomerId, CustomerOrganizationResult, CustomerOrganizationForAdd, CustomerOrganizationForUpdate>,
		IReadOnlyCustomerOrganizationImplementation, ICustomerOrganizationImplementationBase,
		ICustomerImplementation<CustomerOrganizationResult, CustomerOrganizationForAdd, CustomerOrganizationForUpdate> { }
}
