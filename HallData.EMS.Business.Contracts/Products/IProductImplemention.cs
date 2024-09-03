using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
	public interface IReadOnlyProductImplementation<TProductResult> : IReadOnlyProductBaseImplementation<TProductResult>
		where TProductResult: IProductResultBase
	{
	}
	public interface IProductImplementation<TProductResult, in TProductForAdd, in TProductForUpdate> : IReadOnlyProductImplementation<TProductResult>, 
		IProductBaseImplementation<TProductResult, TProductForAdd, TProductForUpdate>
		where TProductResult: IProductResultBase
		where TProductForAdd: IProductForAddBase
		where TProductForUpdate: IProductForUpdateBase
	{ }

	public interface IReadOnlyProductImplementation : IReadOnlyBrandedProductImplementation<ProductResult> { }

	[Service("products")]
	public interface IProductImplementation : IReadOnlyProductImplementation, IBrandedProductImplementation<ProductResult, ProductForAdd, ProductForUpdate> { }
}
