/// <reference path="../../../libraries/jquery/jquery.d.ts" />
module HallData.EMS.ViewModels.Interface {

	export class InterfaceGridModel {
		public interfaceGrid: HallData.UI.Grid.IGrid<any>;

		constructor(
			controller: Controllers.IControllerBase<Controllers.IScope<any>>,
			service: HallData.EMS.Services.Interface.IInterfaceService,
			personalization: UI.Services.IPersonalizationService,
			redirectService: Util.RedirectService,
			routeDictionaryService: Util.RouteDictionaryService,
			viewName: string
		) {
			this.interfaceGrid = new UI.Grid.ViewGrid<Services.Interface.IInterfaceService>(service, personalization, viewName, controller,
				redirectService, routeDictionaryService);
		}
	}
}