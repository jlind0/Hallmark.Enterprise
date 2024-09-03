module HallData.EMS.ApplicationView {
	"use strict";

	import ApplicationViewService = HallData.EMS.Services.ApplicationView.IApplicationViewService;

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class ApplicationViewDisplayController extends ViewController<IApplicationViewDisplayScope> implements IApplicationViewDisplayController {

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"applicationViewService",
			"personalizationService"
		];

		constructor(
			$injector: any,
			$scope: IApplicationViewDisplayScope,
			authentication: Authentication.IAuthenticationProvider,
			private applicationViewService: EMS.Services.ApplicationView.IApplicationViewService,
			private personalizationService: UI.Services.IPersonalizationService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			this.applicationViewService = applicationViewService;
			this.personalizationService = personalizationService;
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("loadapplicationView");
			var isFirstRequest = true;

			return this.applicationViewService.Get(
				Util.Obj.getValue(routeValueDictionary, "applicationViewId"),
				"ApplicationViewResult"
			).then(
				applicationView => this.onLoadSuccess(applicationView),
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

		private onLoadSuccess(applicationView: Service.IQueryResult<Services.ApplicationView.IApplicationView>): void {
			console.log("onLoadSuccess");
			console.log(JSON.stringify(applicationView));
			this.$scope.applicationView = applicationView.Result;
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting applicationView");
			return error;
		}
	}
}