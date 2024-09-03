using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using HallData.Repository;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using System.Data.SqlClient;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Data
{
	public class ProductRepository<TProductResult, TProductForAdd, TProductForUpdate> : ProductBaseRepository<TProductResult, TProductForAdd, TProductForUpdate>,
		IProductRepository<TProductResult, TProductForAdd, TProductForUpdate>
		where TProductResult : IProductResultBase
		where TProductForAdd : IProductForAddBase
		where TProductForUpdate : IProductForUpdateBase
	{
		public ProductRepository(Database db) : base(db) { }

		protected ProductRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectProductQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }


		public virtual Task<QueryResult<BrandResult>> GetBrand(Guid productId, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public virtual Task<QueryResult<JObject>> GetBrandView(Guid productId, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public virtual Task<QueryResults<SupplierResult>> GetSuppliers(Guid productId, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public virtual Task<QueryResults<JObject>> GetSuppliersView(Guid productId, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}

	public class ProductRepository : BrandedProductRepository<ProductResult, ProductForAddBase, ProductForUpdateBase>, IProductRepository
	{
		public ProductRepository(Database db) : base(db) { }


		public Task<QueryResults<TProductResult>> GetProducts<TProductResult>(Guid productId, ProductTypes? productType = null, string viewName = null, Guid? userId = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken)) where TProductResult : IProductResultBase
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetProductsView(Guid productId, ProductTypes? productType = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<TProductResult>> GetParent<TProductResult>(Guid productId, ProductTypes parentProductType, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken)) where TProductResult : IProductResultBase
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> GetParentView(Guid productId, ProductTypes parentProductType, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}
}
