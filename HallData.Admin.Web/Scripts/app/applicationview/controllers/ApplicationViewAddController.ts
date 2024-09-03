module HallData.EMS.ApplicationView {
	"use strict";

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class ApplicationViewAddController extends ViewController<IApplicationViewAddScope> implements IApplicationViewAddController {

		private applicationViewForAdd: EMS.Services.ApplicationView.IApplicationView;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"applicationViewService"
		];

		constructor(
			$injector: any,
			$scope: IApplicationViewAddScope,
			authentication: Authentication.IAuthenticationProvider,
			private applicationViewService: EMS.Services.ApplicationView.IApplicationViewService

			) {
			super($injector, $scope, authentication);
			console.log("constructor");

			$scope.applicationView = this.createModel();
			$scope.isSaving = false;

			this.applicationViewForAdd = <any>{};

			this.applicationViewService = applicationViewService;
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("doLoad: ApplicationViewAddController");

			return this.applicationViewService.GetViewMany("ApplicationViewResult", null, null, null).then(
				applicationView => this.onLoadSuccess(applicationView),
				error => {
					this.onLoadError(error);
					return error;
				});
		}

		private onLoadSuccess(applicationView: Service.IQueryResults<any>): void {
			console.log("onLoadSuccess");

			this.$scope.applicationView = applicationView.Results;
			//console.log("this.$scope.availableInterfaces: " + JSON.stringify(this.$scope.availableInterfaces));
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting data views");
			return error;
		}

		private createModel(): Services.ApplicationView.IApplicationView {
			console.log("createModel");
			return {

			}
		}

		private setApplicationViewForAdd(applicationView: Service.IQueryResult<Services.ApplicationView.IApplicationView>): void {
			console.log("setApplicationViewForAdd");

			//this.applicationViewForAdd.name = this.$scope.applicationView.uidataviews.name;

			console.log("applicationViewForAdd: " + JSON.stringify(this.applicationViewForAdd));
		}

		public add(isValid: boolean, applicationView: Service.IQueryResult<Services.ApplicationView.IApplicationView>): void {
			console.log("add");
			//console.log("applicationView: " + JSON.stringify(this.$scope.applicationView));

			if (isValid) {
				this.$scope.isSaving = true;
				var isFirstRequest = true;

				this.setApplicationViewForAdd(applicationView);

				this.process(() => this.applicationViewService.Add(this.applicationViewForAdd, "ApplicationViewResult").then(
					applicationView => this.onAddSuccess(applicationView),
					error => {
						if (error.status === 403 && isFirstRequest) {
							isFirstRequest = false;
						} else {
							this.onAddError(error);
						}
						return error;
					}
					));
			} else {
				console.log("invalid");
				this.$scope.isSaving = false;
			}
		}

		private onAddSuccess(applicationView: Service.IQueryResult<Services.ApplicationView.IApplicationView>): void {
			console.log("onAddSuccess");
			growl("notice", "Success", "ApplicationView added successfully");

			setTimeout(() => {
				this.redirectService.redirect(this.routeDictionaryService.generateData(("ApplicationViewDetailsDisplay"),
					[
						//{ Key: "applicationViewId", Value: applicationView.Result.uidataviews.dataviewid }
					]));
			}, 1000);
		}

		private onAddError(error: any): void {
			console.log("onAddError: " + JSON.stringify(error));
			var errorParse = $.parseJSON(error.responseText);
			var errorText = errorParse.Message;

			growl("error", "Error", "Error adding applicationView:\n" + errorText);
			this.$scope.isSaving = false;
			this.$scope.$digest();
			return error;
		}
	}
}