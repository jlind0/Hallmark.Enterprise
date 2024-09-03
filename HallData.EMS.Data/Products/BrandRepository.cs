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
	public class BrandRepository : ProductBaseRepository<BrandResult, BrandForAddBase, BrandForUpdateBase>, IBrandRepository
	{
		// TODO: verify
		protected const string SelectBrandQuery = "select * from v_products where [productguid#] = @productguid and [__userguid?] = @__userguid";

		public BrandRepository(Database db) : base(db) { }

		protected BrandRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectProductQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override ProductTypes? ProductType
		{
			get
			{
				return ProductTypes.Brand;
			}
		}

		public Task<QueryResults<BrandResult>> GetBrands(CustomerId customerId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<BrandResult> filter = null, SortContext<BrandResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResults<BrandResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<JObject>> GetBrandsView(CustomerId customerId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResult<BrandResult>> GetBrand(CustomerId customerId, Guid brandId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResult<BrandResult>(cmd, userId, token);
		}

		public Task<QueryResult<JObject>> GetBrandView(CustomerId customerId, Guid brandId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadView(cmd, userId, token);
		}
	}
}
