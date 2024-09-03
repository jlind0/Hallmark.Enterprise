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
	public interface IReadOnlyProductRepository<TProductResult> : IReadOnlyProductBaseRepository<TProductResult>
		where TProductResult: IProductResultBase
	{
		
	}

	public interface IProductRepository<TProductResult, in TProductForAdd, in TProductForUpdate> : IProductBaseRepository<TProductResult, TProductForAdd, TProductForUpdate>, 
		IReadOnlyProductRepository<TProductResult>
		where TProductResult: IProductResultBase
		where TProductForAdd: IProductForAddBase
		where TProductForUpdate: IProductForUpdateBase
	{ }
}
