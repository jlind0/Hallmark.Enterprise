module HallData.EMS.DataView {
	"use strict";

	import DataViewService = HallData.EMS.Services.DataView.IDataViewService;

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class DataViewDisplayController extends ViewController<IDataViewDisplayScope> implements IDataViewDisplayController {

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"dataViewService",
			"personalizationService"
		];

		constructor(
			$injector: any,
			$scope: IDataViewDisplayScope,
			authentication: Authentication.IAuthenticationProvider,
			private dataViewService: EMS.Services.DataView.IDataViewService,
			private personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.dataViewService = dataViewService;
			this.personalizationService = personalizationService;
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("loaddataView");
			var isFirstRequest = true;

			return this.dataViewService.Get(
				Util.Obj.getValue(routeValueDictionary, "dataViewId"),
				"DataViewResult"
			).then(
				dataView => this.onLoadSuccess(dataView),
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

		private onLoadSuccess(dataView: Service.IQueryResult<Services.DataView.IDataView>): void {
			console.log("onLoadSuccess");
			console.log(JSON.stringify(dataView));
			this.$scope.dataView = dataView.Result;
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting dataView");
			return error;
		}
	}
}