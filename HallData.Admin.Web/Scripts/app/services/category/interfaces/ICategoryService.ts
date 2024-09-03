module HallData.EMS.Services.Category {
	"use strict";

	export interface ICategoryService extends Service.IDeleteableService<ICategory>
	{
		GetCategory(filter: HallData.Service.IFilterContext, sort: HallData.Service.ISortContext, page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<ICategory>>;
	}
}