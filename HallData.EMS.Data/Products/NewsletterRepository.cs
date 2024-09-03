using HallData.Data;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;

namespace HallData.EMS.Data
{
	public class NewsletterRepository : BrandedProductRepository<NewsletterResult, NewsletterForAddBase, NewsletterForUpdate>, INewsletterRepository
	{
		protected const string SelectNewsletterQuery = "select * from v_products where [productguid#] = @productguid and [__userguid?] = @__userguid";

		public NewsletterRepository(Database db) : base(db) { }

		protected NewsletterRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectNewsletterQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override ProductTypes? ProductType
		{
			get
			{
				return ProductTypes.Newsletter;
			}
		}
	}
}
