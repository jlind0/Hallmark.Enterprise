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
	public class BrandedProductRepository<TBrandedProductResult, TBrandedProductForAdd, TBrandedProductForUpdate> : ProductRepository<TBrandedProductResult, TBrandedProductForAdd, TBrandedProductForUpdate>,
		IBrandedProductRepository<TBrandedProductResult, TBrandedProductForAdd, TBrandedProductForUpdate>
		where TBrandedProductForAdd : IBrandedProductForAddBase
		where TBrandedProductForUpdate : IBrandedProductForUpdateBase
		where TBrandedProductResult : IBrandedProductResultBase
	{
		public BrandedProductRepository(Database db) : base(db) { }

		protected BrandedProductRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectProductQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		public Task AddSupplier(CustomerId customerId, Guid brandId, Guid productId, SupplierId supplierId, Guid? businessUnitId = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task RemoveSupplier(CustomerId customerId, Guid brandId, Guid productId, SupplierId supplierId, Guid? businessUnitId = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<TBrandedProductResult>> Get(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetSqlQuery);
			PopulateGetCommand(productId, cmd);
			PopulateCustomerId(cmd, customerId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResult<TBrandedProductResult>(cmd, userId, token);
		}

		public Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetSqlQuery);
			PopulateGetCommand(productId, cmd);
			PopulateCustomerId(cmd, customerId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadView(cmd, userId, token);
		}

		public Task<QueryResults<SupplierResult>> GetSuppliers(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetSuppliersView(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
		
	}
}
