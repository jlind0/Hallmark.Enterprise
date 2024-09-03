/// <reference path="../../../libraries/jquery/jquery.d.ts" />
module HallData.EMS.ViewModels.Interface {

	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class InterfaceAttributeGridModel {
		public interfaceAttributeGrid: HallData.UI.Grid.IGrid<any>;

		constructor(
			controller: Controllers.IControllerBase<Controllers.IScope<any>>,
			service: EMS.Services.Interface.IInterfaceService,
			personalization: UI.Services.IPersonalizationService,
			redirectService: Util.RedirectService,
			routeDictionaryService: Util.RouteDictionaryService,
			viewName: string
			) {
			var interfaceId = <number>Util.Obj.getValue(routeValueDictionary, "interfaceId");
			this.interfaceAttributeGrid = new Services.InterfaceAttribute.InterfaceAttributeRecursiveGrid(interfaceId, service, personalization, viewName, controller,
				redirectService, routeDictionaryService);
		}
	}
}