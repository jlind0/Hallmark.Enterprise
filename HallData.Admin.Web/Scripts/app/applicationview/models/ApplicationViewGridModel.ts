/// <reference path="../../../libraries/jquery/jquery.d.ts" />
module HallData.EMS.ViewModels.ApplicationView {

	export class ApplicationViewGridModel {
		public applicationViewGrid: HallData.UI.Grid.IGrid<any>;

		constructor(
			controller: Controllers.IControllerBase<Controllers.IScope<any>>,
			service: HallData.EMS.Services.ApplicationView.IApplicationViewService,
			personalization: UI.Services.IPersonalizationService,
			redirectService: Util.RedirectService,
			routeDictionaryService: Util.RouteDictionaryService,
			viewName: string
		) {
			this.applicationViewGrid = new UI.Grid.ViewGrid<Services.ApplicationView.IApplicationViewService>(service, personalization, viewName, controller,
				redirectService, routeDictionaryService);
		}
	}
}