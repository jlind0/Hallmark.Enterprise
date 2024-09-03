module HallData.EMS.Services.Category {
	"use strict";

	declare var serviceLocation: string;
	
	export class CategoryService<TCategory extends ICategory> extends Service.DeletableService<TCategory> implements ICategoryService {

		private serviceLocation: string;

		//TODO: Need to refactor invalid serviceLocation
		constructor() {
			super(serviceLocation + "categories/");
			console.log("constructor: " + serviceLocation);
			this.serviceLocation = serviceLocation + "categories/View/";
		}

		protected GetUrlPathForKey(customer: ICategory): any {
			console.log("GetUrlPathForKey");
			return customer.Id;
		}

		public GetCategory(filter: HallData.Service.IFilterContext, sort: HallData.Service.ISortContext, page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<ICategory>> {
			console.log("GetCategory");
			return Util.Service.GetQueryResults<ICategory>(this.serviceLocation + "CategoryResult", filter, sort, page);
		}
	}
} 