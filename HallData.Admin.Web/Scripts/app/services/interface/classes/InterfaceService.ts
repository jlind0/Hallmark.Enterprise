module HallData.EMS.Services.Interface {
	import FilterContext = HallData.Service.IFilterContext;
	import SortContext = HallData.Service.ISortContext;
	import PageDescriptor = HallData.Service.IPageDescriptor;
	import QueryResults = HallData.Service.IQueryResults;

	"use strict";

	declare var serviceLocation: string;

	export class InterfaceService extends Service.DeletableService<IInterface> implements IInterfaceService {

		constructor() {
			super(serviceLocation + "interfaces/");
			console.log("constructor");
		}

		GetAttributesWithDownwardRecursion(interfaceId: number, viewName?: string, filter?: FilterContext, sort?: SortContext, page?: PageDescriptor): JQueryPromise<QueryResults<any>> {
			return Util.Service.GetQueryResults<any>(this.ServiceLocation + interfaceId + "/Attributes/recurse/down/TypedView/" + Util.String.emptyIfNullOrWhitespace(viewName), filter, sort, page);
		}
	}
}