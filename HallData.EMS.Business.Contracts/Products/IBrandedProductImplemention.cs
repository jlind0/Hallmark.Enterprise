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
	public interface IReadOnlyBrandedProductImplementation<TProductResult> : IReadOnlyProductImplementation<TProductResult>
		where TProductResult : IBrandedProductResultBase
	{  
	}

	public interface IBrandedProductImplementation<TProductResult, in TProductForAdd, in TProductForUpdate> : IReadOnlyBrandedProductImplementation<TProductResult>,
		IProductImplementation<TProductResult, TProductForAdd, TProductForUpdate>
		where TProductResult : IBrandedProductResultBase
		where TProductForAdd : IBrandedProductForAddBase
		where TProductForUpdate : IBrandedProductForUpdateBase
	{
	}
}
