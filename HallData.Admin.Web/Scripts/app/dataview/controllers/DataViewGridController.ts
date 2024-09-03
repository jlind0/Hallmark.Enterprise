module HallData.EMS.DataView {
	"use strict";

	export class DataViewGridController extends ViewController<IDataViewGridScope> implements IDataViewGridController{

		public static $inject = [
			"$injector",
			"$scope",
			"$http",
			"authentication",
			"dataViewService",
			"personalizationService"
		];

		public grid: ViewModels.DataView.DataViewGridModel;

		constructor(
			$injector: any,
			$scope: IDataViewGridScope,
			$http: ng.IHttpService,
			authentication: Authentication.IAuthenticationProvider,
			dataViewService: Services.DataView.IDataViewService,
			personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.grid = new ViewModels.DataView.DataViewGridModel(this, dataViewService, personalizationService, this.redirectService, this.routeDictionaryService, "DataViewResults");
			ko.applyBindings(this.grid, $("#dataviewsgrid")[0]);
		}
	}
}