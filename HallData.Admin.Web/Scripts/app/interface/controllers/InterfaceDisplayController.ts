module HallData.EMS.Interface {
	"use strict";

	import InterfaceService = HallData.EMS.Services.Interface.IInterfaceService;
	import InterfaceAttributeService = HallData.EMS.Services.InterfaceAttribute.IInterfaceAttributeService;

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class InterfaceDisplayController extends ViewController<IInterfaceDisplayScope> implements IInterfaceDisplayController {

		public grid: ViewModels.Interface.InterfaceAttributeGridModel;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"interfaceService",
			"interfaceAttributeService",
			"personalizationService"
		];

		constructor(
			$injector: any,
			$scope: IInterfaceDisplayScope,
			authentication: Authentication.IAuthenticationProvider,
			private interfaceService: EMS.Services.Interface.IInterfaceService,
			private interfaceAttributeService: EMS.Services.InterfaceAttribute.IInterfaceAttributeService,
			private personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.interfaceService = interfaceService;
			this.interfaceAttributeService = interfaceAttributeService;
			this.personalizationService = personalizationService;
			this.initializeGrid();
		}

		private initializeGrid(): void {
			console.log("initializeGrid");
			this.grid = new ViewModels.Interface.InterfaceAttributeGridModel(
				this, this.interfaceService, this.personalizationService,
				this.redirectService, this.routeDictionaryService, "InterfaceAttributeResult");
			ko.applyBindings(this.grid, $("#interfaceattributesgrid").first()[0]);
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("loadinterfaceObj");
			var isFirstRequest = true;

			return this.interfaceService.Get(
				Util.Obj.getValue(routeValueDictionary, "interfaceId"),
				"InterfaceResult"
			).then(
				interfaceObj => this.onLoadSuccess(interfaceObj),
				error => {
					if (error.status === 403 && isFirstRequest) {
						isFirstRequest = false;
					} else {
						this.onLoadError(error);
					}
					return error;
				}
			);
		}

		private onLoadSuccess(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("onLoadSuccess");
			console.log(JSON.stringify(interfaceObj));
			this.$scope.interfaceObj = interfaceObj.Result;
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting interface");
			return error;
		}
	}
}