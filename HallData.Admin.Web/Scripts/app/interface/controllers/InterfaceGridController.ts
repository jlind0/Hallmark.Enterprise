module HallData.EMS.Interface {
	"use strict";

	export class InterfaceGridController extends ViewController<IInterfaceGridScope> implements IInterfaceGridController{

		public static $inject = [
			"$injector",
			"$scope",
			"$http",
			"authentication",
			"interfaceService",
			"personalizationService"
		];

		public grid: ViewModels.Interface.InterfaceGridModel;

		constructor(
			$injector: any,
			$scope: IInterfaceGridScope,
			$http: ng.IHttpService,
			authentication: Authentication.IAuthenticationProvider,
			interfaceService: Services.Interface.IInterfaceService,
			personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.grid = new ViewModels.Interface.InterfaceGridModel(this, interfaceService, personalizationService, this.redirectService, this.routeDictionaryService, "InterfaceResults");
			ko.applyBindings(this.grid, $("#interfacesgrid").first()[0]);
		}
	}
}