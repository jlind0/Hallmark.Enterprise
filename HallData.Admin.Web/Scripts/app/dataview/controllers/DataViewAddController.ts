module HallData.EMS.DataView {
	"use strict";

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class DataViewAddController extends ViewController<IDataViewAddScope> implements IDataViewAddController {

		private dataViewForAdd: EMS.Services.DataView.IDataView;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"dataViewService"
		];

		constructor(
			$injector: any,
			$scope: IDataViewAddScope,
			authentication: Authentication.IAuthenticationProvider,
			private dataViewService: EMS.Services.DataView.IDataViewService

			) {
			super($injector, $scope, authentication);
			console.log("constructor");

			$scope.dataView = this.createModel();
			$scope.isSaving = false;

			this.dataViewForAdd = <any>{};

			this.dataViewService = dataViewService;
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("doLoad: DataViewAddController");

			return this.dataViewService.GetViewMany("DataViewResult", null, null, null).then(
				dataView => this.onLoadSuccess(dataView),
				error => {
					this.onLoadError(error);
					return error;
				});
		}

		private onLoadSuccess(dataView: Service.IQueryResults<any>): void {
			console.log("onLoadSuccess");

			this.$scope.dataView = dataView.Results;
			//console.log("this.$scope.availableInterfaces: " + JSON.stringify(this.$scope.availableInterfaces));
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting data views");
			return error;
		}

		private createModel(): Services.DataView.IDataView {
			console.log("createModel");
			return {

			}
		}

		private setDataViewForAdd(dataView: Service.IQueryResult<Services.DataView.IDataView>): void {
			console.log("setDataViewForAdd");

			this.dataViewForAdd.uidataviews.name = this.$scope.dataView.uidataviews.name;

			console.log("dataViewForAdd: " + JSON.stringify(this.dataViewForAdd));
		}

		public add(isValid: boolean, dataView: Service.IQueryResult<Services.DataView.IDataView>): void {
			console.log("add");
			//console.log("dataView: " + JSON.stringify(this.$scope.dataView));

			if (isValid) {
				this.$scope.isSaving = true;
				var isFirstRequest = true;

				this.setDataViewForAdd(dataView);

				this.process(() => this.dataViewService.Add(this.dataViewForAdd, "DataViewResult").then(
					dataView => this.onAddSuccess(dataView),
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

		private onAddSuccess(dataView: Service.IQueryResult<Services.DataView.IDataView>): void {
			console.log("onAddSuccess");
			growl("notice", "Success", "DataView added successfully");

			setTimeout(() => {
				this.redirectService.redirect(this.routeDictionaryService.generateData(("DataViewDetailsDisplay"),
					[
						{ Key: "dataViewId", Value: dataView.Result.uidataviews.dataviewid }
					]));
			}, 1000);
		}

		private onAddError(error: any): void {
			console.log("onAddError: " + JSON.stringify(error));
			var errorParse = $.parseJSON(error.responseText);
			var errorText = errorParse.Message;

			growl("error", "Error", "Error adding dataView:\n" + errorText);
			this.$scope.isSaving = false;
			this.$scope.$digest();
			return error;
		}
	}
}