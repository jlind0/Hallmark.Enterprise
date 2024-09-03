module HallData.EMS.InterfaceAttribute {
	"use strict";

	export class InterfaceAttributeGridController extends ViewController<IInterfaceAttributeGridScope> implements IInterfaceAttributeGridController{

		public static $inject = [
			"$injector",
			"$scope",
			"$http",
			"authentication",
			"interfaceService",
			//"interfaceAttributeService",
			"personalizationService"
		];

		public grid: ViewModels.Interface.InterfaceAttributeGridModel;

		constructor(
			$injector: any,
			$scope: IInterfaceAttributeGridScope,
			$http: ng.IHttpService,
			authentication: Authentication.IAuthenticationProvider,
			interfaceService: Services.Interface.IInterfaceService,
			personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.grid = new ViewModels.Interface.InterfaceAttributeGridModel(this, interfaceService, personalizationService, this.redirectService, this.routeDictionaryService, "InterfaceAttributeResult");
			ko.applyBindings(this.grid, $("#interfaceattributesgrid").first()[0]);
		}
	}
}