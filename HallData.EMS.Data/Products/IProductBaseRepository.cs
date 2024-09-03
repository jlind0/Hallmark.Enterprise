using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
	public interface IReadOnlyProductBaseRepository<TProductResult> : IReadOnlyRepository<Guid, TProductResult>
		where TProductResult : IProductBaseResultBase
	{
		Task<QueryResults<TProductResult>> GetByCustomer(CustomerId customerId,
			string viewName = null, Guid? userId = null,
			FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null,
			PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		Task<QueryResults<JObject>> GetByCustomerView(CustomerId customerId, string viewName = null, Guid? userId = null, FilterContext filter = null,
			SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		Task<QueryResults<TProductResult>> GetAllByCustomer(CustomerId customerId, string viewName = null,
			Guid? userId = null, FilterContext<TProductResult> filter = null,
			SortContext<TProductResult> sort = null, PageDescriptor page = null,
			CancellationToken token = default(CancellationToken));

		Task<QueryResults<JObject>> GetAllByCustomerView(CustomerId customerId, string viewName = null, Guid? userId = null, FilterContext filter = null,
			SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		Task<QueryResults<TProductResult>> GetByBusinessUnit(Guid businessUnitId,
			string viewName = null, Guid? userId = null,
			FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null,
			PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		Task<QueryResults<JObject>> GetByBusinessUnitView(Guid businessUnitId, string viewName = null, Guid? userId = null, FilterContext filter = null,
			SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
	}

	public interface IProductBaseRepository<TProductResult, in TProductForAdd, in TProductForUpdate> : 
		IDeletableRepository<Guid, TProductResult, TProductForAdd, TProductForUpdate>, 
		IReadOnlyProductBaseRepository<TProductResult>
		where TProductResult: IProductBaseResultBase
		where TProductForAdd: IProductBaseForAddBase
		where TProductForUpdate: IProductBaseForUpdateBase
	{
		
	}

	public interface IReadOnlyProductContactMechanismRepository : IReadOnlyContactHolderRepository<ProductContactMechanismId, Guid, ProductContactMechanismKey> { }
	public interface IProductContactMechanismRepository : IReadOnlyProductContactMechanismRepository, IContactHolderRepository<ProductContactMechanismId, Guid, ProductContactMechanismKey> { }
}