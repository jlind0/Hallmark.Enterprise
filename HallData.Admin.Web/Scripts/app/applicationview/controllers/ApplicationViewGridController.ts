module HallData.EMS.ApplicationView {
	"use strict";

	export class ApplicationViewGridController extends ViewController<IApplicationViewGridScope> implements IApplicationViewGridController{

		public static $inject = [
			"$injector",
			"$scope",
			"$http",
			"authentication",
			"applicationViewService",
			"personalizationService"
		];

		public grid: ViewModels.ApplicationView.ApplicationViewGridModel;

		constructor(
			$injector: any,
			$scope: IApplicationViewGridScope,
			$http: ng.IHttpService,
			authentication: Authentication.IAuthenticationProvider,
			applicationViewService: Services.ApplicationView.IApplicationViewService,
			personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.grid = new ViewModels.ApplicationView.ApplicationViewGridModel(this, applicationViewService, personalizationService, this.redirectService, this.routeDictionaryService, "ApplicationViewResult");
			ko.applyBindings(this.grid, $("#applicationviewsgrid").first()[0]);
		}
	}
}