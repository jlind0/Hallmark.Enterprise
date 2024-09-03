module HallData.EMS.Services.Interface {
	import FilterContext = HallData.Service.IFilterContext;
	import SortContext = HallData.Service.ISortContext;
	import PageDescriptor = HallData.Service.IPageDescriptor;
	import QueryResults = HallData.Service.IQueryResults;
	"use strict";

	export interface IInterfaceService extends Service.IDeleteableService<IInterface> {
		GetAttributesWithDownwardRecursion(interfaceId: number, viewName?: string, filter?: FilterContext, sort?: SortContext, page?: PageDescriptor): JQueryPromise<QueryResults<any>>;
	}
}