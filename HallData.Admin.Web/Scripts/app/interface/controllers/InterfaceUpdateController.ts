module HallData.EMS.Interface {
	"use strict";

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class InterfaceUpdateController extends ViewController<IInterfaceUpdateScope> implements IInterfaceUpdateController {

		private status: EMS.Services.IStatusType[];
		private interfaceForUpdate: EMS.Services.Interface.IInterface;
		private availableInterfaces: EMS.Services.Interface.IInterface;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"interfaceService"
		];

		constructor(
			$injector: any,
			$scope: IInterfaceUpdateScope,
			authentication: Authentication.IAuthenticationProvider,
			private interfaceService: EMS.Services.Interface.IInterfaceService
		) {
			super($injector, $scope, authentication);
			console.log("constructor");

			//$scope.interfaceObj = <HallData.EMS.Service.Interface.IInterface>;
			$scope.isSaving = false;

			this.interfaceService = interfaceService;
			this.interfaceForUpdate = <any>{};

			this.getAvailableInterfaces();
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("doLoad");

			return this.interfaceService.Get(
				Util.Obj.getValue(routeValueDictionary, "interfaceId"),
				"InterfaceResult"
			).then(
				interfaceObj => this.onLoadSuccess(interfaceObj),
				error => {
					this.onLoadError(error);
					return error;
				});
		}

		private onLoadSuccess(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("onLoadSuccess");
			console.log("interfaceObj: " + JSON.stringify(interfaceObj));

			this.$scope.interfaceObj = interfaceObj.Result;
			console.log("this.$scope.interfaceObj: " + JSON.stringify(this.$scope.interfaceObj));
			//growl("notice", "Success", "Interface updated successfully"); // do we need this?
		}

		protected getAvailableInterfaces(): JQueryPromise<void> {
			console.log("getAvailableInterfaces");

			return this.interfaceService.GetViewMany("InterfaceResult", null, null, null).then(
				interfaceObj => this.onInterfaceLoadSuccess(interfaceObj),
				error => {
					this.onLoadError(error);
					return error;
				});
		}

		private onInterfaceLoadSuccess(interfaceObj: Service.IQueryResults<any>): void {
			console.log("onInterfaceLoadSuccess");

			for (var x = 0; x < this.$scope.interfaceObj.RelatedInterfaces.length; x++) {
				for (var i = 0; i < interfaceObj.Results.length; i++) {
					if (this.$scope.interfaceObj.RelatedInterfaces[x].InterfaceId ==  interfaceObj.Results[i].InterfaceId) {
						interfaceObj.Results[i].ticked = true;
					}
				}
			}

			this.$scope.availableInterfaces = interfaceObj.Results;
			//console.log("this.$scope.availableInterfaces: " + JSON.stringify(this.$scope.availableInterfaces));
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting interface");
			return error;
		}

		private setInterfaceForUpdate(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("setCustomerForUpdate");

			this.interfaceForUpdate.Name = this.$scope.interfaceObj.Name;
			this.interfaceForUpdate.DisplayName = this.$scope.interfaceObj.DisplayName;
			this.interfaceForUpdate.RelatedInterfaces = this.$scope.interfaceObj.RelatedInterfaces;

			console.log("interfaceForUpdate: " + JSON.stringify(this.interfaceForUpdate));
		}

		public update(isValid: boolean, interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("update");
			if (isValid) {
				this.$scope.isSaving = true;

				this.setInterfaceForUpdate(interfaceObj);
				
				this.process(() => this.interfaceService.Update(this.interfaceForUpdate, "InterfaceResult").then(
					interfaceObj => this.onUpdateSuccess(interfaceObj),
					error => {
						this.onUpdateError(error);
						return error;
					}
				));
			} else {
				console.log("invalid");
				this.$scope.isSaving = false;
			}
		}

		private onUpdateSuccess(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("onUpdateSuccess");
			growl("notice", "Success", "Interface updated successfully");

			setTimeout(() => {
				this.redirectService.redirect(this.routeDictionaryService.generateData(("InterfaceDetailsDisplay"),
					[
						{ Key: "interfaceId", Value: interfaceObj.Result.InterfaceId }
					]));
			}, 1000);
		}

		private onUpdateError(error: any): void {
			console.log("onUpdateError: " + JSON.stringify(error));
			var errorParse = $.parseJSON(error.responseText);
			var errorText = errorParse.Message;

			growl("error", "Error", "Error updating interface:\n" + errorText);
			this.$scope.isSaving = false;
			this.$scope.$digest();
			return error;
		}
	}
}