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
using System.Data.Common;

namespace HallData.EMS.Data
{
	public class ProductBaseRepository<TProductResult, TProductForAdd, TProductForUpdate> : DeletableRepository<Guid, TProductResult, TProductForAdd, TProductForUpdate>,
		IProductBaseRepository<TProductResult, TProductForAdd, TProductForUpdate>
		where TProductResult : IProductBaseResultBase
		where TProductForAdd : IProductBaseForAddBase
		where TProductForUpdate : IProductBaseForUpdateBase
	{
		protected const string SelectAllProductsProcedure = "usp_select_products";
		protected const string SelectProductQuery = "select * from v_products where [productguid#] = @productguid and [__userguid?] = @__userguid";
		protected const string InsertProductProcedure = "usp_insert_products";
		protected const string UpdateProductProcedure = "usp_update_products";
		protected const string DeleteProductProcedure = "usp_delete_products";
		protected const string ChangeStatusProductProcedure = "usp_changestatus_products";

		public ProductBaseRepository(Database db)
			: base(db, SelectAllProductsProcedure, SelectProductQuery, InsertProductProcedure, UpdateProductProcedure, DeleteProductProcedure, ChangeStatusProductProcedure) { }

		protected ProductBaseRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectProductQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected virtual void PopulateCustomerId(DbCommand cmd, CustomerId customerId)
		{
			cmd.AddParameter("partyGuid", customerId.PartyGuid);

			if (customerId.CustomerOfPartyGuid != null)
			{
				cmd.AddParameter("customerOfGuid", customerId.CustomerOfPartyGuid);
			}
		}

		protected virtual void PopulateBrandId(DbCommand cmd, Guid? brandId)
		{
			if (brandId != null)
			{
				cmd.AddParameter("brandGuId", brandId);
			}
		}

		protected virtual void PopulateBusinessUnitId(DbCommand cmd, Guid? businessUnitId)
		{
			if (businessUnitId != null)
			{
				cmd.AddParameter("businessUnitGuid", businessUnitId);
			}
		}

		protected virtual void PopulateProductId(DbCommand cmd, Guid? productId)
		{
			if (productId != null)
			{
				cmd.AddParameter("productGuid", productId);
			}
		}

		protected virtual void PopulateProductType(DbCommand cmd)
		{
			var pt = ProductType;
			if (pt != null)
				cmd.AddParameter("producttypeid", (int)pt.Value);
		}

		protected override void PopulateGetAllStoredProcedure(DbCommand cmd)
		{
			PopulateProductType(cmd);
			base.PopulateGetAllStoredProcedure(cmd);
		}

		protected virtual ProductTypes? ProductType
		{
			get { return null; }
		}

		protected override void PopulateDeleteCommand(Guid id, DbCommand cmd)
		{
			cmd.AddParameter("productguid", id);
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, Guid id)
		{
			cmd.AddParameter("productguid", id);
		}

		protected override Guid ReadKeyFromScalarReturnObject(object obj, TProductForAdd view)
		{
			return (Guid)obj;
		}

		protected override void PopulateGetCommand(Guid id, DbCommand cmd)
		{
			PopulateProductType(cmd);
			cmd.AddParameter("productguid", id);
		}

		public Task<QueryResults<TProductResult>> GetByCustomer(CustomerId customerId, string viewName = null, Guid? userId = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			return ReadQueryResults<TProductResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<JObject>> GetByCustomerView(CustomerId customerId, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<TProductResult>> GetAllByCustomer(CustomerId customerId, string viewName = null, Guid? userId = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			return ReadQueryResults<TProductResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<JObject>> GetAllByCustomerView(CustomerId customerId, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<TProductResult>> GetByBusinessUnit(Guid businessUnitId, string viewName = null, Guid? userId = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResults<TProductResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<JObject>> GetByBusinessUnitView(Guid businessUnitId, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}
	}
}
