module HallData.EMS.Services.InterfaceAttribute {
	"use strict";

	declare var serviceLocation: string;

	export class InterfaceAttributeRecursiveGrid extends UI.Grid.ViewGrid<Services.Interface.IInterfaceService> {

		constructor(protected interfaceId: number, service: Services.Interface.IInterfaceService, personalizationService: UI.Services.IPersonalizationService, viewName: string,
			controller: Controllers.IControllerBase<Controllers.IScope<any>>, redirectService: Util.RedirectService, routeDictionaryService: Util.RouteDictionaryService) {
			super(service, personalizationService, viewName, controller, redirectService, routeDictionaryService);
			console.log("constructor");
		}

		protected getItems(page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<any>> {
			var promise = <JQueryPromise<HallData.Service.IQueryResults<any>>>this.service.GetAttributesWithDownwardRecursion(this.interfaceId, this.viewName, this.filterContext, this.sortContext, page);
			return promise.then(items => items, error =>
				this.processError(error)
				);
		}

	}
} 