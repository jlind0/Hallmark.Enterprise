/// <reference path="../../../libraries/jquery/jquery.d.ts" />
module HallData.EMS.ViewModels.DataView {

	export class DataViewGridModel {
		public dataViewGrid: HallData.UI.Grid.IGrid<any>;

		constructor(
			controller: Controllers.IControllerBase<Controllers.IScope<any>>,
			service: HallData.EMS.Services.DataView.IDataViewService,
			personalization: UI.Services.IPersonalizationService,
			redirectService: Util.RedirectService,
			routeDictionaryService: Util.RouteDictionaryService,
			viewName: string
		) {
			this.dataViewGrid = new UI.Grid.ViewGrid<Services.DataView.IDataViewService>(service, personalization, viewName, controller,
				redirectService, routeDictionaryService);
		}
	}
}